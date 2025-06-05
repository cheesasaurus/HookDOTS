using System;
using HookDOTS.API.Utilities;
using Unity.Entities;

namespace HookDOTS.API.HookRegistration;

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
            throw new Exception($"HookDOTS: null SystemTypeIndex for {systemType.FullName}");
        }
        else
        {
            var confirmedSystemName = TypeManager.GetSystemType(systemTypeIndex).FullName;
            LogUtil.LogDebug($"HookDOTS: registered OnUpdate prefix hook for: {confirmedSystemName}");
        }

        var handle = new HookHandle()
        {
            Value = _idGenerator.NextId(),
            HookType = HookType.System_OnUpdate_Prefix,
            SystemTypeIndex = systemTypeIndex,
        };

        return RegisterHook(systemTypeIndex, handle, registryEntry);
    }

}