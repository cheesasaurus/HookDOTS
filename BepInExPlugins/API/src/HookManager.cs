using System;
using System.Collections.Generic;
using Unity.Entities;
using BepInEx.Logging;
using HookDOTS.API.HookRegistration;

namespace HookDOTS.API;

public static class HookManager
{
    public static Bus Bus = new(); // todo: maybe move this out of HookManager, and make the manager internal
    private static bool _initialized = false;
    private static bool _isGameReadyForRegistration = false;

    private static HookRegistry _hookRegistry;

    ////////////////////////////////////////////////////////////////////

    #region Setup / Teardown

    internal static void Initialize()
    {
        if (_initialized)
        {
            return;
        }
        _hookRegistry = new HookRegistry();
        Bus.GameReadyForRegistration += HandleGameReadyForRegistration;
        _initialized = true;
    }

    internal static void UnInitialize()
    {
        if (!_initialized)
        {
            return;
        }
        Bus.GameReadyForRegistration -= HandleGameReadyForRegistration;
        _hookRegistry = null;
        _isGameReadyForRegistration = false;
        _initialized = false;
    }

    #endregion

    ////////////////////////////////////////////////////////////////////

    #region Handlers

    private static void HandleGameReadyForRegistration()
    {
        _isGameReadyForRegistration = true;
    }

    private static Dictionary<SystemTypeIndex, bool> _restoreEnabledAfterPrefixSkip_System_OnUpdate = new();
    private static Dictionary<SystemTypeIndex, bool> _didPrefixExpectSystemToRun = new();

    unsafe internal static void HandleSystemUpdatePrefix(SystemState* systemState)
    {
        var systemTypeIndex = systemState->m_SystemTypeIndex;
        bool wouldRunSystem = systemState->Enabled && systemState->ShouldRunSystem();
        var subRegistry = _hookRegistry.SubRegistry_System_OnUpdate_Prefix;

        bool shouldStopExecutingPrefixesAndSkipTheOriginal = false;
        foreach (var registryEntry in subRegistry.GetEntriesInReverseRegistrationOrder(systemTypeIndex))
        {
            try
            {
                if (!wouldRunSystem && registryEntry.Options.OnlyWhenSystemRuns)
                {
                    continue;
                }

                if (false == registryEntry.Hook(systemState))
                {
                    shouldStopExecutingPrefixesAndSkipTheOriginal = true;
                    break;
                }
            }
            catch (Exception ex)
            {
                registryEntry.Log.LogError(ex);
                continue;
            }
            
        }

        if (shouldStopExecutingPrefixesAndSkipTheOriginal)
        {
            _restoreEnabledAfterPrefixSkip_System_OnUpdate[systemTypeIndex] = systemState->Enabled;
            systemState->Enabled = false;
        }
        _didPrefixExpectSystemToRun[systemTypeIndex] = wouldRunSystem && !shouldStopExecutingPrefixesAndSkipTheOriginal;
    }

    unsafe internal static void HandleSystemUpdatePostfix(SystemState* systemState)
    {
        // todo: error handling

        // todo: onlyWhenSystemRuns option

        var systemTypeIndex = systemState->m_SystemTypeIndex;
        if (_restoreEnabledAfterPrefixSkip_System_OnUpdate.ContainsKey(systemTypeIndex))
        {
            systemState->Enabled = _restoreEnabledAfterPrefixSkip_System_OnUpdate[systemTypeIndex];
            _restoreEnabledAfterPrefixSkip_System_OnUpdate.Remove(systemTypeIndex);
        }

        // todo: run postfix hooks
    }

    #endregion

    ////////////////////////////////////////////////////////////////////

    internal static HookRegistrar NewHookRegistrar(string id, ManualLogSource log)
    {
        var staging = new HookRegistryStaging(id, _hookRegistry, Bus, _isGameReadyForRegistration, log);
        return new HookRegistrar(id, staging, log);
    }
    
}

