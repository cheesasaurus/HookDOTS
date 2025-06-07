using Unity.Entities;
using BepInEx.Logging;
using HookDOTS.HookRegistration;

namespace HookDOTS;

internal static class HookManager
{
    private static Bus _bus = Bus.Instance;
    private static bool _initialized = false;
    private static bool _isGameReadyForRegistration = false;

    private static HookRegistry _hookRegistry;
    private static Executor_System_OnUpdate _executor_System_OnUpdate;
    private static Executor_WhenCreatedWorldsContain _executor_WhenCreatedWorldsContain;

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
        _executor_WhenCreatedWorldsContain = new Executor_WhenCreatedWorldsContain(
            _hookRegistry.SubRegistry_WhenCreatedWorldsContainAny,
            _hookRegistry.SubRegistry_WhenCreatedWorldsContainAll,
            new WorldWatcher()
        );
        _bus.EventGameReadyForHooking += HandleGameReadyForRegistration;
        _bus.EventWorldsMayHaveChanged += HandleWorldsMayHaveChanged;
        _bus.CommandRunHooks_WorldsCreated += HandlerCommandRunHooks_WorldsCreated;
        _initialized = true;
    }

    internal static void UnInitialize()
    {
        if (!_initialized)
        {
            return;
        }
        _bus.EventGameReadyForHooking -= HandleGameReadyForRegistration;
        _bus.EventWorldsMayHaveChanged -= HandleWorldsMayHaveChanged;
        _bus.CommandRunHooks_WorldsCreated -= HandlerCommandRunHooks_WorldsCreated;
        _hookRegistry = null;
        _executor_System_OnUpdate = null;
        _executor_WhenCreatedWorldsContain = null;
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

    unsafe internal static void HandleWorldsMayHaveChanged()
    {
        _executor_WhenCreatedWorldsContain.ExecuteAndRemoveHooks();
    }

    unsafe internal static void HandlerCommandRunHooks_WorldsCreated()
    {
        _executor_WhenCreatedWorldsContain.ExecuteAndRemoveHooks();
    }

    #endregion

    ////////////////////////////////////////////////////////////////////

    internal static HookRegistrar NewHookRegistrar(string id, ManualLogSource log)
    {
        var staging = new HookRegistryStaging(id, _hookRegistry, _bus, _isGameReadyForRegistration, log);
        return new HookRegistrar(id, staging, log);
    }
    
}

