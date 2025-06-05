using System;
using System.Collections.Generic;
using BepInEx.Logging;
using HookDOTS.API.Hooks;
using HookDOTS.API.Utilities;

namespace HookDOTS.API.HookRegistration;

internal class HookRegistryStaging
{
    private string _id;
    private HookRegistry _hookRegistry;
    private Bus _bus;
    private ManualLogSource _log;
    private bool _canRegister = false;

    private List<HookHandle> _registeredHookHandles = new();
    private Queue<RegistryEntries.System_OnUpdate_Prefix> _pendingRegistrations_System_OnUpdate_Prefix = new();

    internal HookRegistryStaging(string id, HookRegistry hookRegistry, Bus bus, bool isGameReadyForRegistration, ManualLogSource log)
    {
        _id = id;
        _hookRegistry = hookRegistry;
        _canRegister = isGameReadyForRegistration;
        _bus = bus;
        _log = log;
        _bus.GameReadyForRegistration += HandleGameReadyForRegistration;
        // todo: way to cleanup event handler
    }

    internal void UnregisterRegisteredHooks()
    {
        foreach (var hookHandle in _registeredHookHandles)
        {
            _hookRegistry.UnregisterHook(hookHandle);
        }
        _registeredHookHandles.Clear();
    }

    internal void CancelPendingRegistrations()
    {
        _pendingRegistrations_System_OnUpdate_Prefix.Clear();
    }

    internal void HandleGameReadyForRegistration()
    {
        _canRegister = true;
        LogUtil.LogDebug($"HookDOTS: processing pending hook registrations for {_id}");
        ProcessPendingRegistrations_System_OnUpdate_Prefix();
    }

    internal void ProcessPendingRegistrations_System_OnUpdate_Prefix()
    {
        while (_pendingRegistrations_System_OnUpdate_Prefix.Count != 0)
        {
            var registryEntry = _pendingRegistrations_System_OnUpdate_Prefix.Dequeue();
            try
            {
                RegisterHook_System_OnUpdate_Prefix(registryEntry);
            }
            catch (Exception ex)
            {
                LogUtil.LogError(ex);
            }
        }
    }

    internal void RegisterHook_System_OnUpdate_Prefix(System_OnUpdate_Prefix.Hook hook, Il2CppSystem.Type systemType, System_OnUpdate_Prefix.Options options)
    {
        var registryEntry = new RegistryEntries.System_OnUpdate_Prefix(hook, systemType, options, _log);
        if (_canRegister)
        {
            RegisterHook_System_OnUpdate_Prefix(registryEntry);
        }
        else
        {
            LogUtil.LogDebug($"HookDOTS: added a pending hook registration for {_id}");
            _pendingRegistrations_System_OnUpdate_Prefix.Enqueue(registryEntry);
        }
    }

    private void RegisterHook_System_OnUpdate_Prefix(RegistryEntries.System_OnUpdate_Prefix entry)
    {
        var handle = _hookRegistry.SubRegistry_System_OnUpdate_Prefix.RegisterHook(entry);
        _registeredHookHandles.Add(handle);
    }

}
