using System;
using BepInEx;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using HookDOTS.API;
using ProjectM.Gameplay.Systems;
using VRisingMods.Core.Utilities;

namespace ExamplePlugin;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
[BepInDependency("HookDOTS.API")]
[BepInDependency("HookDOTS.VRisingBootstrapper")]
public class ExamplePlugin : BasePlugin
{
    Harmony _harmony;
    MainEntryPoint _systemHooksEntry;

    public override void Load()
    {
        // Plugin startup logic
        LogUtil.Init(Log);
        Log.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} version {MyPluginInfo.PLUGIN_VERSION} is loaded!");

        // Harmony patching
        _harmony = new Harmony(MyPluginInfo.PLUGIN_GUID);
        _harmony.PatchAll(System.Reflection.Assembly.GetExecutingAssembly());

        // register hooks (checking attributes)
        _systemHooksEntry = new MainEntryPoint(MyPluginInfo.PLUGIN_GUID);
        _systemHooksEntry.RegisterHooks();

        // procedurally register some hooks too
        var registrar = _systemHooksEntry.HookRegistrar;
        //registrar.RegisterHook_System_OnUpdate_Prefix<DealDamageSystem>(MyHookWithSkip);
        registrar.RegisterHook_System_OnUpdate_Prefix<DealDamageSystem>(MyHook);
    }

    public override bool Unload()
    {
        // unregister hooks like this
        _systemHooksEntry.UnregisterHooks();

        // Harmony unpatching
        _harmony?.UnpatchSelf();
        return true;
    }

    private TimeSpan twoSeconds = new TimeSpan(hours: 0, minutes: 0, seconds: 2);

    private DateTime nextTime1 = DateTime.MinValue;
    private bool MyHook()
    {
        if (DateTime.Now < nextTime1)
        {
            return true;
        }
        nextTime1 = DateTime.Now.Add(twoSeconds);
        LogUtil.LogInfo($"[{DateTime.Now}] MyHook executing. (debounce 2 seconds)");
        return true;
    }

    private DateTime nextTime2 = DateTime.MinValue;
    private bool MyHookWithSkip()
    {
        if (DateTime.Now < nextTime2)
        {
            return false;
        }
        nextTime2 = DateTime.Now.Add(twoSeconds);
        LogUtil.LogInfo($"[{DateTime.Now}] MyHookWithSkip executing. (debounce 2 seconds)");
        return false;
    }

}
