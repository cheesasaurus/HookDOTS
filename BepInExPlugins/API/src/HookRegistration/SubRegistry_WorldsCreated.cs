using System;
using System.Collections.Generic;
using System.Linq;
using HookDOTS.Utilities;
using Unity.Entities;

namespace HookDOTS.HookRegistration;


internal abstract class SubRegistry_WhenCreatedWorldsContain<TRegistryEntry>
{
    protected Dictionary<HookHandle, TRegistryEntry> _hooks = new();

    internal abstract HookHandle RegisterHook(TRegistryEntry registryEntry);

    internal void UnregisterHook(HookHandle hookHandle)
    {
        _hooks.Remove(hookHandle);
    }

    protected HookHandle RegisterHook(HookHandle handle, TRegistryEntry registryEntry)
    {
        _hooks.Add(handle, registryEntry);
        return handle;
    }

    protected string FormattedWorldNames(ISet<string> worldNames)
    {
        return StringUtil.StringifyEnumerableOfStrings(worldNames);
    }

}
