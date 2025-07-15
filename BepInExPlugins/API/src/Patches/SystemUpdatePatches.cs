using HarmonyLib;
using Unity.Entities;

namespace HookDOTS.Patches;

[HarmonyPatch]
unsafe internal class SystemUpdatePatches
{
    /// <remarks>
    /// SystemBase.Update internally calls
    /// (SystemBase.Enabled && SystemBase.ShouldRunSystem),
    /// then proceeds to run SystemBase.OnUpdate if true.
    /// </remarks>
    [HarmonyPatch(typeof(SystemBase), nameof(SystemBase.Update))]
    [HarmonyPrefix]
    unsafe internal static bool SystemManaged_MightUpdate_Prefix(SystemBase __instance)
    {
        return HookManager.HandleSystemUpdatePrefix(__instance.CheckedState());
    }

    [HarmonyPatch(typeof(SystemBase), nameof(SystemBase.Update))]
    [HarmonyPostfix]
    unsafe internal static void SystemManaged_MightUpdate_Postfix(SystemBase __instance)
    {
        HookManager.HandleSystemUpdatePostfix(__instance.CheckedState());
    }

    /// <remarks>
    /// WorldUnmanagedImpl.UnmanagedUpdate internally calls
    /// (SystemState->Enabled && SystemState->ShouldRunSystem),
    /// then proceeds to run SystemBaseRegistry.CallOnUpdate if true.
    /// SystemBaseRegistry (despite its name) contains an UnmanagedSystemTypeRegistryData.
    /// UnmanagedSystemTypeRegistryData.GetSystemDelegates is used to find both managed and burst delegates,
    /// for any of the system OnSomething methods.
    /// </remarks>
    [HarmonyPatch(typeof(WorldUnmanagedImpl), nameof(WorldUnmanagedImpl.UnmanagedUpdate))]
    [HarmonyPrefix]
    unsafe internal static bool SystemUnmanaged_MightUpdate_Prefix(void* pSystemState)
    {
        return HookManager.HandleSystemUpdatePrefix((SystemState*)pSystemState);
    }

    [HarmonyPatch(typeof(WorldUnmanagedImpl), nameof(WorldUnmanagedImpl.UnmanagedUpdate))]
    [HarmonyPostfix]
    unsafe internal static void SystemUnmanaged_MightUpdate_Postfix(void* pSystemState)
    {
        HookManager.HandleSystemUpdatePostfix((SystemState*)pSystemState);
    }

}