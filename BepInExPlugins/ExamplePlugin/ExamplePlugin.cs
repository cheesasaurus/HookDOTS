using System;
using BepInEx;
using BepInEx.Logging;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using ProjectM.Gameplay.Systems;
using Unity.Entities;

namespace ExamplePlugin;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
[BepInDependency("HookDOTS.API")]
[BepInDependency("HookDOTS.VRisingBootstrapper")]
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
        var registrar = _hookDOTS.HookRegistrar;
        //registrar.RegisterHook_System_OnUpdate_Prefix<DealDamageSystem>(MyHookWithSkip);
        registrar.RegisterHook_System_OnUpdate_Prefix<DealDamageSystem>(MyHook);
        
    }

    public override bool Unload()
    {
        // unregister hooks
        _hookDOTS.UnregisterHooks();

        // Harmony unpatching
        _harmony?.UnpatchSelf();
        return true;
    }

    
    private TimeSpan twoSeconds = new TimeSpan(hours: 0, minutes: 0, seconds: 2);

    private DateTime nextTime1 = DateTime.MinValue;
    unsafe private bool MyHook(SystemState* systemState)
    {
        if (DateTime.Now < nextTime1)
        {
            return true;
        }
        nextTime1 = DateTime.Now.Add(twoSeconds);
        Log.LogInfo($"[{DateTime.Now}] MyHook executing. (debounce 2 seconds)");
        return true;
    }

    private DateTime nextTime2 = DateTime.MinValue;
    unsafe private bool MyHookWithSkip(SystemState* systemState)
    {
        // return false to skip  further prefixes and the original
        if (DateTime.Now < nextTime2)
        {
            return false;
        }
        nextTime2 = DateTime.Now.Add(twoSeconds);
        Log.LogInfo($"[{DateTime.Now}] MyHookWithSkip executing. (debounce 2 seconds)");
        return false;
    }

}
