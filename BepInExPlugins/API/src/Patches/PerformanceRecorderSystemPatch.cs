using HarmonyLib;
using Unity.Entities;

namespace HookDOTS.API.Patches;

[HarmonyPatch]
unsafe internal class PerformanceRecorderSystemPatch
{
    static bool _initialized = false;
    static bool _wasStaticRecordingEnabled = false;

    internal static void UnInitialize()
    {
        if (!_initialized)
        {
            return;
        }
        PerformanceRecorderSystem.StaticRecordingEnabled = _wasStaticRecordingEnabled;
        _wasStaticRecordingEnabled = false;
        _initialized = false;
    }

    [HarmonyPatch(typeof(PerformanceRecorderSystem), nameof(PerformanceRecorderSystem.OnUpdate))]
    [HarmonyPrefix]
    internal static bool PerformanceRecorderSystem_OnUpdate_Prefix(PerformanceRecorderSystem __instance)
    {
        // todo: maybe move some of this to an Initialize method
        if (_initialized)
        {
            return false;
        }
        _wasStaticRecordingEnabled = PerformanceRecorderSystem.StaticRecordingEnabled;
        PerformanceRecorderSystem.StaticRecordingEnabled = true;
        _initialized = true;
        // unless desired, Disable PerformanceRecorderSystem.OnUpdate() call as it would do actual data recording and log output.
        return _wasStaticRecordingEnabled;
    }

    [HarmonyPatch(typeof(PerformanceRecorderSystem), nameof(PerformanceRecorderSystem.StartSystem))]
    [HarmonyPrefix]
    internal static void PerformanceRecorderSystem_StartSystem_Prefix(SystemState* systemState)
    {
        HookManager.HandleSystemUpdatePrefix(systemState);
    }

    [HarmonyPatch(typeof(PerformanceRecorderSystem), nameof(PerformanceRecorderSystem.EndSystem))]
    [HarmonyPostfix]
    internal static void PerformanceRecorderSystem_EndSystem_Postfix(SystemState* systemState)
    {
        HookManager.HandleSystemUpdatePostfix(systemState);
    }

}