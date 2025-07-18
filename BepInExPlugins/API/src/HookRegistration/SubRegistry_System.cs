using System;
using System.Collections.Generic;
using Unity.Entities;

namespace HookDOTS.HookRegistration;


internal abstract class SubRegistry_System<TRegistryEntry>
{
    protected Dictionary<SystemTypeIndex, Dictionary<HookHandle, TRegistryEntry>> _hooksBySystem = new();
    protected ICollection<TRegistryEntry> _emptyCollection = Array.Empty<TRegistryEntry>();

    internal abstract HookHandle RegisterHook(TRegistryEntry registryEntry);

    internal void UnregisterHook(HookHandle hookHandle)
    {
        _hooksBySystem[hookHandle.SystemTypeIndex].Remove(hookHandle);
    }

    // HarmonyX runs hooks in the order that they were registered, so we aim to do the same
    internal IEnumerable<TRegistryEntry> GetEntriesInRegistrationOrder(SystemTypeIndex systemTypeIndex)
    {
        // todo: OrderedDictionary kind of thing. Problem is that it's not generic, so using a regular Dictionary for now.
        // it seems to just work
        if (!_hooksBySystem.TryGetValue(systemTypeIndex, out var hooks))
        {
            return _emptyCollection;
        }
        return hooks.Values;        
    }

    protected HookHandle RegisterHook(SystemTypeIndex systemTypeIndex, HookHandle handle, TRegistryEntry registryEntry)
    {
        // ensure we have a dictionary for that system
        Dictionary<HookHandle, TRegistryEntry> hooksForSystem;
        if (_hooksBySystem.ContainsKey(systemTypeIndex))
        {
            hooksForSystem = _hooksBySystem[systemTypeIndex];
        }
        else
        {
            hooksForSystem = new Dictionary<HookHandle, TRegistryEntry>();
            _hooksBySystem.Add(systemTypeIndex, hooksForSystem);
        }

        // register the hook
        hooksForSystem.Add(handle, registryEntry);
        return handle;
    }

}