using HarmonyLib;
using HookDOTS.API;
using HookDOTS.API.Utilities;
using Unity.Entities;

namespace HookDOTS.VRisingBootstrapper.Patches;

[HarmonyPatch]
public static class TypeManagerPatch
{
    private static bool _initialized = false;

    [HarmonyPatch(typeof(TypeManager), nameof(TypeManager.Initialize))]
    [HarmonyPostfix]
    public static void Initialize()
    {
        if (_initialized)
        {
            return;
        }
        _initialized = true;
        LogUtil.LogInfo("TypeManager is ready.");
        Bus.Instance.TriggerGameReadyForHooking();
    }
}
