using HarmonyLib;
using HookDOTS.API;
using Unity.Entities;
using VRisingMods.Core.Utilities;

namespace HookDOTS.VRisingBootstrapper.Patches;

[HarmonyPatch]
public static class TimeableComponentSystemGroupPatch
{
    private static bool _initialized = false;

    [HarmonyPatch(typeof(TimeableComponentSystemGroup), nameof(TimeableComponentSystemGroup.OnUpdate))]
    [HarmonyPostfix]
    public static void Initialize()
    {
        if (_initialized)
        {
            return;
        }
        _initialized = true;
        LogUtil.LogInfo("Game has bootstrapped. Worlds and systems now exist.");
        HookManager.Bus.TriggerGameReadyForRegistration();
    }
}

