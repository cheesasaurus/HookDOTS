using Unity.Entities;
using BepInEx.Logging;
using HookDOTS.API.HookRegistration;

namespace HookDOTS.API;

internal static class HookManager
{
    private static Bus _bus = Bus.Instance;
    private static bool _initialized = false;
    private static bool _isGameReadyForRegistration = false;

    private static HookRegistry _hookRegistry;
    private static Executor_System_OnUpdate _executor_System_OnUpdate;

    ////////////////////////////////////////////////////////////////////

    #region Setup / Teardown

    internal static void Initialize()
    {
        if (_initialized)
        {
            return;
        }
        _hookRegistry = new HookRegistry();
        _executor_System_OnUpdate = new Executor_System_OnUpdate(
            prefixSubRegistry: _hookRegistry.SubRegistry_System_OnUpdate_Prefix,
            postfixSubRegistry: _hookRegistry.SubRegistry_System_OnUpdate_Postfix
        );
        _bus.GameReadyForHooking += HandleGameReadyForRegistration;
        _initialized = true;
    }

    internal static void UnInitialize()
    {
        if (!_initialized)
        {
            return;
        }
        _bus.GameReadyForHooking -= HandleGameReadyForRegistration;
        _hookRegistry = null;
        _executor_System_OnUpdate = null;
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

    unsafe internal static void HandleSystemUpdatePrefix(SystemState* systemState)
    {
        _executor_System_OnUpdate.ExecutePrefixHooks(systemState);
    }

    unsafe internal static void HandleSystemUpdatePostfix(SystemState* systemState)
    {
        _executor_System_OnUpdate.ExecutePostfixHooks(systemState);
    }

    #endregion

    ////////////////////////////////////////////////////////////////////

    internal static HookRegistrar NewHookRegistrar(string id, ManualLogSource log)
    {
        var staging = new HookRegistryStaging(id, _hookRegistry, _bus, _isGameReadyForRegistration, log);
        return new HookRegistrar(id, staging, log);
    }
    
}

