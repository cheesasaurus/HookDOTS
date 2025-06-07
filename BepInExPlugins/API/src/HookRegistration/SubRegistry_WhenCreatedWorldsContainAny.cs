using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HookDOTS.Utilities;
using Unity.Entities;

namespace HookDOTS.HookRegistration;

internal class SubRegistry_WhenCreatedWorldsContainAny : SubRegistry_WhenCreatedWorldsContain<RegistryEntries.WhenCreatedWorldsContainAny>
{
    private IdGenerator _idGenerator;
    internal SubRegistry_WhenCreatedWorldsContainAny(IdGenerator idGenerator)
    {
        _idGenerator = idGenerator;
    }

    // HarmonyX runs hooks in the order that they were registered, so we aim to do the same
    internal IEnumerable<RegistryEntries.WhenCreatedWorldsContainAny> ExtractMatchesInRegistrationOrder(IWorldWatcher worldWatcher)
    {
        var matchedKeys = _hooks.Keys.Where(k => worldWatcher.AreAnyWorldsCreated(_hooks[k].WorldNames));
        var matchedValues = matchedKeys.Select(_hooks.GetValueOrDefault);
        matchedKeys.Select(_hooks.Remove); // unregister matches
        return matchedValues;
    }

    internal override HookHandle RegisterHook(RegistryEntries.WhenCreatedWorldsContainAny registryEntry)
    {
        var handle = new HookHandle()
        {
            Value = _idGenerator.NextId(),
            HookType = HookType.WhenCreatedWorldsContainAny,
            SystemTypeIndex = SystemTypeIndex.Null,
        };

        RegisterHook(handle, registryEntry);
        LogUtil.LogDebug(MultiLineRegistrationLogEntry(registryEntry, handle));
        return handle;
    }

    protected string MultiLineRegistrationLogEntry(RegistryEntries.WhenCreatedWorldsContainAny registryEntry, HookHandle handle)
    {
        var sb = new StringBuilder();
        sb.AppendLine($"HookDOTS registered hook#{handle.Value}");
        sb.AppendLine($"    registrant: {registryEntry.RegistrantId}");
        sb.AppendLine($"    target:     WhenCreatedWorldsContainAny([{FormattedWorldNames(registryEntry.WorldNames)}])");
        sb.Append($"    invoke:     {registryEntry.Hook.FullName()}");
        return sb.ToString();
    }
    
}
