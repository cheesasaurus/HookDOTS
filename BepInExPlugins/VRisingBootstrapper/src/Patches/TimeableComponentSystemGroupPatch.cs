using HarmonyLib;
using HookDOTS.API;
using Unity.Entities;

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
        VRisingBootstrapperPlugin.LogInstance.LogInfo("Game has bootstrapped. Worlds and systems now exist.");
        Bus.Instance.TriggerGameReadyForHooking();
    }
}

