using BepInEx;
using BepInEx.Logging;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using HookDOTS.API;
using HookDOTS.API.Hooks;
using ProjectM;
using Unity.Entities;

namespace ExamplePlugin;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
[BepInDependency("HookDOTS.API")]
public class ExamplePlugin : BasePlugin
{
    public static ManualLogSource LogInstance { get; private set; }
    Harmony _harmony;
    HookDOTS.API.HookDOTS _hookDOTS;

    public override void Load()
    {
        // Plugin startup logic
        LogInstance = Log;
        Log.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} version {MyPluginInfo.PLUGIN_VERSION} is loaded!");

        // Harmony patching
        _harmony = new Harmony(MyPluginInfo.PLUGIN_GUID);
        _harmony.PatchAll(System.Reflection.Assembly.GetExecutingAssembly());

        // register hooks from attributes. (you can see some used in src/patches/EcsSystemUpdatePrefix_ExamplePatch.cs)
        _hookDOTS = new HookDOTS.API.HookDOTS(MyPluginInfo.PLUGIN_GUID, Log);
        _hookDOTS.RegisterHooks();

        // It's also possible to procedurally register hooks
        ProcedurallyRegisterHooks();
    }

    unsafe private void ProcedurallyRegisterHooks()
    {
        var hook = System_OnUpdate_Prefix.CreateHook(MyProcedurallyRegisteredHook);
        _hookDOTS.HookRegistrar.RegisterHook_System_OnUpdate_Prefix<TakeDamageInSunSystem_Server>(hook);
    }

    public override bool Unload()
    {
        // be sure to call Dispose! This will unregister hooks and clean up to avoid memory leaks.
        _hookDOTS.Dispose();

        // Harmony unpatching
        _harmony?.UnpatchSelf();
        return true;
    }

    private Throttle throttle = new Throttle(seconds: 2);
    unsafe private bool MyProcedurallyRegisteredHook(SystemState* systemState)
    {
        if (throttle.CheckAndTrigger())
        {
            return false;
        }
        Log.LogInfo($"MyProcedurallyRegisteredHook executing. (limit once per 2 seconds)");
        return false;
    }

}
