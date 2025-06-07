using System;
using System.Collections.Generic;
using BepInEx.Logging;


namespace HookDOTS.HookRegistration;

internal static class RegistryEntries
{
    internal record System_OnUpdate_Prefix(
        Hooks.System_OnUpdate_Prefix.Hook Hook,
        Il2CppSystem.Type SystemType,
        Hooks.System_OnUpdate_Prefix.Options Options,
        ManualLogSource Log,
        String RegistrantId
    );

    internal record System_OnUpdate_Postfix(
        Hooks.System_OnUpdate_Postfix.Hook Hook,
        Il2CppSystem.Type SystemType,
        Hooks.System_OnUpdate_Postfix.Options Options,
        ManualLogSource Log,
        String RegistrantId
    );

    internal record WhenCreatedWorldsContainAny(
        Hooks.WhenCreatedWorldsContainAny.Hook Hook,
        ISet<string> WorldNames,
        ManualLogSource Log,
        String RegistrantId
    );

    internal record WhenCreatedWorldsContainAll(
        Hooks.WhenCreatedWorldsContainAll.Hook Hook,
        ISet<string> WorldNames,
        ManualLogSource Log,
        String RegistrantId
    );

}
