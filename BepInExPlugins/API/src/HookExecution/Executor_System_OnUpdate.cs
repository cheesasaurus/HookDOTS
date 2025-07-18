using System;
using System.Collections.Generic;
using HookDOTS.API;
using Unity.Entities;

namespace HookDOTS.HookRegistration;


//
// Summary:
//     Executor_System_OnUpdate is responsible for executing
//     System.OnUpdate prefix/postfix hooks.
internal class Executor_System_OnUpdate
{
    private SubRegistry_System_OnUpdate_Prefix _prefixSubRegistry;
    private SubRegistry_System_OnUpdate_Postfix _postfixSubRegistry;
    private Dictionary<World, WorldboundExecutor> _worldboundExecutors = new();

    public Executor_System_OnUpdate(SubRegistry_System_OnUpdate_Prefix prefixSubRegistry, SubRegistry_System_OnUpdate_Postfix postfixSubRegistry)
    {
        _prefixSubRegistry = prefixSubRegistry;
        _postfixSubRegistry = postfixSubRegistry;
    }
    
    private WorldboundExecutor GetOrCreateWorldboundExecutor(World world)
    {
        if (_worldboundExecutors.ContainsKey(world))
        {
            return _worldboundExecutors[world];
        }
        var executor = new WorldboundExecutor(
            world: world,
            prefixSubRegistry: _prefixSubRegistry,
            postfixSubRegistry: _postfixSubRegistry
        );
        _worldboundExecutors.Add(world, executor);
        return executor;
    }

    unsafe internal bool ExecutePrefixHooks(SystemState* systemState)
    {
        var executor = GetOrCreateWorldboundExecutor(systemState->World);
        return executor.ExecutePrefixHooks(systemState);
    }

    unsafe internal void ExecutePostfixHooks(SystemState* systemState)
    {
        var executor = GetOrCreateWorldboundExecutor(systemState->World);
        executor.ExecutePostfixHooks(systemState);
    }

    // A system class can have instances in multiple worlds. so we partition things per-world.
    private class WorldboundExecutor
    {
        private World _world;
        private SubRegistry_System_OnUpdate_Prefix _prefixSubRegistry;
        private SubRegistry_System_OnUpdate_Postfix _postfixSubRegistry;

        public WorldboundExecutor(World world, SubRegistry_System_OnUpdate_Prefix prefixSubRegistry, SubRegistry_System_OnUpdate_Postfix postfixSubRegistry)
        {
            _world = world;
            _prefixSubRegistry = prefixSubRegistry;
            _postfixSubRegistry = postfixSubRegistry;
        }
        private static Dictionary<SystemTypeIndex, bool> _didPrefixExpectSystemToRun = new();

        unsafe internal bool ExecutePrefixHooks(SystemState* systemState)
        {
            var systemTypeIndex = systemState->m_SystemTypeIndex;
            bool wouldRunSystem = systemState->Enabled && systemState->ShouldRunSystem();

            bool shouldSkipTheOriginal = false;
            foreach (var registryEntry in _prefixSubRegistry.GetEntriesInRegistrationOrder(systemTypeIndex))
            {
                try
                {
                    if (!wouldRunSystem && registryEntry.Options.OnlyWhenSystemRuns)
                    {
                        continue;
                    }
                    if (registryEntry.Options.Throttle?.CheckAndTrigger() ?? false)
                    {
                        continue;
                    }

                    if (false == registryEntry.Hook.Invoke(systemState))
                    {
                        shouldSkipTheOriginal = true;
                    }
                }
                catch (Exception ex)
                {
                    registryEntry.Log.LogError(ex);
                    continue;
                }

            }
            _didPrefixExpectSystemToRun[systemTypeIndex] = wouldRunSystem && !shouldSkipTheOriginal;
            return shouldSkipTheOriginal ? AfterDetours.SkipOriginalMethod : AfterDetours.OkToRunOriginalMethod;
        }

        unsafe internal void ExecutePostfixHooks(SystemState* systemState)
        {
            var systemTypeIndex = systemState->m_SystemTypeIndex;
            var didSystemProbablyRun = _didPrefixExpectSystemToRun[systemTypeIndex];

            foreach (var registryEntry in _postfixSubRegistry.GetEntriesInRegistrationOrder(systemTypeIndex))
            {
                try
                {
                    if (!didSystemProbablyRun && registryEntry.Options.OnlyWhenSystemRuns)
                    {
                        continue;
                    }
                    if (registryEntry.Options.Throttle?.CheckAndTrigger() ?? false)
                    {
                        continue;
                    }
                    registryEntry.Hook.Invoke(systemState);
                }
                catch (Exception ex)
                {
                    registryEntry.Log.LogError(ex);
                    continue;
                }
            }
        }

    }

}
