using System;
using System.Collections.Generic;
using BepInEx.Logging;

namespace HookDOTS.API.HookRegistration;

// todo: split this up
internal class HookRegistryStaging
{
    private string _id;
    private HookRegistry _hookRegistry;
    private Bus _bus;
    private ManualLogSource _log;
    private bool _canRegister = false;

    private List<HookHandle> _registeredHookHandles = new();
    private Queue<RegistryEntries.System_OnUpdate_Prefix> _pendingRegistrations_System_OnUpdate_Prefix = new();
    private Queue<RegistryEntries.System_OnUpdate_Postfix> _pendingRegistrations_System_OnUpdate_Postfix = new();

    internal HookRegistryStaging(string id, HookRegistry hookRegistry, Bus bus, bool isGameReadyForRegistration, ManualLogSource log)
    {
        _id = id;
        _hookRegistry = hookRegistry;
        _canRegister = isGameReadyForRegistration;
        _bus = bus;
        _log = log;
        _bus.GameReadyForHooking += HandleGameReadyForRegistration;
    }

    public void Dispose()
    {
        _bus.GameReadyForHooking -= HandleGameReadyForRegistration;
        UnregisterRegisteredHooks();
        CancelPendingRegistrations();
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
        _pendingRegistrations_System_OnUpdate_Postfix.Clear();
    }

    internal void HandleGameReadyForRegistration()
    {
        _canRegister = true;
        _log.LogDebug($"HookDOTS: processing pending hook registrations for {_id}");
        ProcessPendingRegistrations_System_OnUpdate_Prefix();
        ProcessPendingRegistrations_System_OnUpdate_Postfix();
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
                _log.LogError(ex);
            }
        }
    }

    internal void ProcessPendingRegistrations_System_OnUpdate_Postfix()
    {
        while (_pendingRegistrations_System_OnUpdate_Postfix.Count != 0)
        {
            var registryEntry = _pendingRegistrations_System_OnUpdate_Postfix.Dequeue();
            try
            {
                RegisterHook_System_OnUpdate_Postfix(registryEntry);
            }
            catch (Exception ex)
            {
                _log.LogError(ex);
            }
        }
    }

    internal void RegisterHook_System_OnUpdate_Prefix(Hooks.System_OnUpdate_Prefix.HookSignature hook, Il2CppSystem.Type systemType, Hooks.System_OnUpdate_Prefix.Options options)
    {
        var registryEntry = new RegistryEntries.System_OnUpdate_Prefix(hook, systemType, options, _log, _id);
        if (_canRegister)
        {
            RegisterHook_System_OnUpdate_Prefix(registryEntry);
        }
        else
        {
            _log.LogDebug($"HookDOTS: added a pending hook registration for {_id}");
            _pendingRegistrations_System_OnUpdate_Prefix.Enqueue(registryEntry);
        }
    }

    private void RegisterHook_System_OnUpdate_Prefix(RegistryEntries.System_OnUpdate_Prefix entry)
    {
        var handle = _hookRegistry.SubRegistry_System_OnUpdate_Prefix.RegisterHook(entry);
        _registeredHookHandles.Add(handle);
    }

    internal void RegisterHook_System_OnUpdate_Postfix(Hooks.System_OnUpdate_Postfix.HookSignature hook, Il2CppSystem.Type systemType, Hooks.System_OnUpdate_Postfix.Options options)
    {
        var registryEntry = new RegistryEntries.System_OnUpdate_Postfix(hook, systemType, options, _log, _id);
        if (_canRegister)
        {
            RegisterHook_System_OnUpdate_Postfix(registryEntry);
        }
        else
        {
            _log.LogDebug($"HookDOTS: added a pending hook registration for {_id}");
            _pendingRegistrations_System_OnUpdate_Postfix.Enqueue(registryEntry);
        }
    }

    private void RegisterHook_System_OnUpdate_Postfix(RegistryEntries.System_OnUpdate_Postfix entry)
    {
        var handle = _hookRegistry.SubRegistry_System_OnUpdate_Postfix.RegisterHook(entry);
        _registeredHookHandles.Add(handle);
    }

}
