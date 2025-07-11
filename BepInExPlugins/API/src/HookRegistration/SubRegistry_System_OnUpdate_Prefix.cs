using System;
using System.Text;
using HookDOTS.Utilities;
using Unity.Entities;

namespace HookDOTS.HookRegistration;

internal class SubRegistry_System_OnUpdate_Prefix : SubRegistry_System<RegistryEntries.System_OnUpdate_Prefix>
{
    private IdGenerator _idGenerator;
    internal SubRegistry_System_OnUpdate_Prefix(IdGenerator idGenerator)
    {
        _idGenerator = idGenerator;
    }

    internal override HookHandle RegisterHook(RegistryEntries.System_OnUpdate_Prefix registryEntry)
    {
        var systemType = registryEntry.SystemType;
        var systemTypeIndex = TypeManager.GetSystemTypeIndex(systemType);
        if (systemTypeIndex.Equals(SystemTypeIndex.Null))
        {
            throw new Exception($"null SystemTypeIndex for {systemType.FullName}");
        }

        var handle = new HookHandle()
        {
            Value = _idGenerator.NextId(),
            HookType = HookType.System_OnUpdate_Prefix,
            SystemTypeIndex = systemTypeIndex,
        };

        RegisterHook(systemTypeIndex, handle, registryEntry);
        var confirmedSystemName = TypeManager.GetSystemType(systemTypeIndex).FullName;
        LogUtil.LogDebug(MultiLineRegistrationLogEntry(registryEntry, handle, confirmedSystemName));
        return handle;
    }

    protected string MultiLineRegistrationLogEntry(RegistryEntries.System_OnUpdate_Prefix registryEntry, HookHandle handle, String confirmedSystemName)
    {
        var sb = new StringBuilder();
        sb.AppendLine($"HookDOTS registered hook#{handle.Value}.");
        sb.AppendLine($"    registrant: {registryEntry.RegistrantId}");
        sb.AppendLine($"    target:     {confirmedSystemName}.OnUpdate");
        sb.AppendLine($"    precedence: PREFIX");
        sb.Append($"    detour to:  {registryEntry.Hook.FullName()}");
        return sb.ToString();
    }

}