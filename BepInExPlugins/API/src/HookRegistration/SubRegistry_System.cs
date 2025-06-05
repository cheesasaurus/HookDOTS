using System;
using System.Collections.Generic;
using Unity.Entities;

namespace HookDOTS.API.HookRegistration;


internal abstract class SubRegistry_System<TRegistryEntry>
{
    protected Dictionary<SystemTypeIndex, Dictionary<HookHandle, TRegistryEntry>> _hooksBySystem = new();
    protected ICollection<TRegistryEntry> _emptyCollection = Array.Empty<TRegistryEntry>();

    internal abstract HookHandle RegisterHook(TRegistryEntry registryEntry);

    internal void UnregisterHook(HookHandle hookHandle)
    {
        _hooksBySystem[hookHandle.SystemTypeIndex].Remove(hookHandle);
    }

    // todo: more performant way of dealing with this
    internal ICollection<TRegistryEntry> GetEntriesInReverseRegistrationOrder(SystemTypeIndex systemTypeIndex)
    {
        if (!_hooksBySystem.ContainsKey(systemTypeIndex))
        {
            return _emptyCollection;
        }
        // todo: OrderedDictionary kind of thing. Problem is that it's not generic, so just using a regular Dictionary for now
        //return lookup[systemTypeIndex].Values.Reverse();
        return _hooksBySystem[systemTypeIndex].Values; // todo: actually reverse it, and also optimize things
    }

    protected HookHandle RegisterHook(SystemTypeIndex systemTypeIndex, HookHandle handle, TRegistryEntry registryEntry)
    {
        // ensure we have a registry for that system
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