using BepInEx;
using BepInEx.Logging;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;

namespace HookDOTS.VRisingBootstrapper;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
[BepInDependency("HookDOTS.API")]
public class VRisingBootstrapperPlugin : BasePlugin
{
    public static ManualLogSource LogInstance { get; private set; }
    Harmony _harmony;

    public override void Load()
    {
        // Plugin startup logic
        LogInstance = Log;
        Log.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} version {MyPluginInfo.PLUGIN_VERSION} is loaded!");

        // Harmony patching
        _harmony = new Harmony(MyPluginInfo.PLUGIN_GUID);
        _harmony.PatchAll(System.Reflection.Assembly.GetExecutingAssembly());
    }

    public override bool Unload()
    {
        _harmony?.UnpatchSelf();
        return true;
    }

}
