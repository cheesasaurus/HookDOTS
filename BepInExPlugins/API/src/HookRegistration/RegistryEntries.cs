using System;
using BepInEx.Logging;


namespace HookDOTS.API.HookRegistration;

internal static class RegistryEntries
{
    internal record System_OnUpdate_Prefix(
        Hooks.System_OnUpdate_Prefix.HookFunction Hook,
        Il2CppSystem.Type SystemType,
        Hooks.System_OnUpdate_Prefix.Options Options,
        ManualLogSource Log,
        String RegistrantId
    );

    internal record System_OnUpdate_Postfix(
        Hooks.System_OnUpdate_Postfix.HookFunction Hook,
        Il2CppSystem.Type SystemType,
        Hooks.System_OnUpdate_Postfix.Options Options,
        ManualLogSource Log,
        String RegistrantId
    );

}
