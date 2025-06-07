using BepInEx;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using HookDOTS.Patches;
using HookDOTS.Utilities;
using Unity.Entities;

namespace HookDOTS.API;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class ApiPlugin : BasePlugin
{
    Harmony _harmony;

    public override void Load()
    {
        // Plugin startup logic
        LogUtil.Init(Log);
        Log.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} version {MyPluginInfo.PLUGIN_VERSION} is loaded!");

        // Harmony patching
        _harmony = new Harmony(MyPluginInfo.PLUGIN_GUID);
        _harmony.PatchAll(System.Reflection.Assembly.GetExecutingAssembly());

        // set up HookManager
        HookManager.Initialize();

        // currently, only the type manager is needed to begin hooking,
        // but this may change as features are added
        TypeManager.Initialize();
        Bus.Instance.TriggerEventGameReadyForHooking();
    }

    public override bool Unload()
    {
        HookManager.UnInitialize();
        PerformanceRecorderSystemPatch.UnInitialize();

        _harmony?.UnpatchSelf();
        return true;
    }

}
