using BepInEx;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using HookDOTS.API.Patches;
using VRisingMods.Core.Utilities;

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
    }

    public override bool Unload()
    {
        HookManager.UnInitialize();
        PerformanceRecorderSystemPatch.UnInitialize();

        _harmony?.UnpatchSelf();
        return true;
    }

}
