using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HookDOTS.Utilities;
using Unity.Entities;

namespace HookDOTS.HookRegistration;

internal class SubRegistry_WhenCreatedWorldsContainAll : SubRegistry_WhenCreatedWorldsContain<RegistryEntries.WhenCreatedWorldsContainAll>
{
    private IdGenerator _idGenerator;
    internal SubRegistry_WhenCreatedWorldsContainAll(IdGenerator idGenerator)
    {
        _idGenerator = idGenerator;
    }

    // HarmonyX runs hooks in the order that they were registered, so we aim to do the same
    internal IEnumerable<RegistryEntries.WhenCreatedWorldsContainAll> ExtractMatchesInRegistrationOrder(IWorldWatcher worldWatcher)
    {
        var matchedKeys = _hooks.Keys.Where(k => worldWatcher.AreAllWorldsCreated(_hooks[k].WorldNames));
        var matchedValues = matchedKeys.Select(_hooks.GetValueOrDefault).ToList();
        foreach (var k in matchedKeys) // don't use LINQ for this, it has deferred execution
        { 
            _hooks.Remove(k);
        }
        return matchedValues;
    }

    internal override HookHandle RegisterHook(RegistryEntries.WhenCreatedWorldsContainAll registryEntry)
    {
        var handle = new HookHandle()
        {
            Value = _idGenerator.NextId(),
            HookType = HookType.WhenCreatedWorldsContainAll,
            SystemTypeIndex = SystemTypeIndex.Null,
        };

        RegisterHook(handle, registryEntry);
        LogUtil.LogDebug(MultiLineRegistrationLogEntry(registryEntry, handle));
        return handle;
    }

    protected string MultiLineRegistrationLogEntry(RegistryEntries.WhenCreatedWorldsContainAll registryEntry, HookHandle handle)
    {
        var sb = new StringBuilder();
        sb.AppendLine($"HookDOTS registered hook#{handle.Value}");
        sb.AppendLine($"    registrant: {registryEntry.RegistrantId}");
        sb.AppendLine($"    target:     WhenCreatedWorldsContainAll([{FormattedWorldNames(registryEntry.WorldNames)})]");
        sb.Append($"    invoke:     {registryEntry.Hook.FullName()}");
        return sb.ToString();
    }
    
}
