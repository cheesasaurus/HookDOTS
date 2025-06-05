using BepInEx.Logging;


namespace HookDOTS.API.HookRegistration;

internal static class RegistryEntries
{
    internal record System_OnUpdate_Prefix(
        Hooks.System_OnUpdate_Prefix.Hook Hook,
        Il2CppSystem.Type SystemType,
        Hooks.System_OnUpdate_Prefix.Options Options,
        ManualLogSource Log
    );

}
