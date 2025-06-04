using HarmonyLib;
using HookDOTS.API;
using Stunlock.Core;
using VRisingMods.Core.Utilities;

namespace HookDOTS.VRisingBootstrapper.Patches;

[HarmonyPatch]
public static class WorldBootrapPatch
{
    [HarmonyPatch(typeof(WorldBootstrapUtilities), nameof(WorldBootstrapUtilities.AddSystemsToWorld))]
    [HarmonyPostfix]
    public static void Initialize()
    {
        LogUtil.LogInfo("Game has bootstrapped. Worlds and systems now exist.");
        HookManager.Bus.TriggerGameReadyForRegistration();
    }
}

