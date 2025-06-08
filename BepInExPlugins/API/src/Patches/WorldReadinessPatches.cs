using HarmonyLib;
using HookDOTS;
using HookDOTS.Utilities;
using Unity.Entities;

namespace ExamplePlugin.Patches;

[HarmonyPatch]
unsafe internal class WorldReadinessPatches
{

    // In VRising, this is used for the "Default World" and the "Server" world
    [HarmonyPatch(typeof(ScriptBehaviourUpdateOrder), nameof(ScriptBehaviourUpdateOrder.AppendWorldToCurrentPlayerLoop))]
    [HarmonyPostfix]
    static void WorldAppendedToCurrentPlayerLoop(World world)
    {
        // LogUtil.LogError($"AppendWorldToCurrentPlayerLoop '{world.Name}' isCreated:{world.IsCreated}");
        Bus.Instance.TriggerEventWorldsMayHaveChanged();
    }

    // (This doesn't seem to be used in VRising)
    [HarmonyPatch(typeof(ScriptBehaviourUpdateOrder), nameof(ScriptBehaviourUpdateOrder.AppendWorldToPlayerLoop))]
    [HarmonyPostfix]
    static void WorldAppendedToSpecificPlayerLoop(World world)
    {
        //LogUtil.LogError($"AppendWorldToPlayerLoop '{world.Name}' isCreated:{world.IsCreated}");
        Bus.Instance.TriggerEventWorldsMayHaveChanged();
    }

}