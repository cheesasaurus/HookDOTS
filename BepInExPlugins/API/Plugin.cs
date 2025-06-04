using System;
using BepInEx;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using ProjectM.Gameplay.Systems;
using HookDOTS.API.Patches;
using VRisingMods.Core.Utilities;

namespace HookDOTS.API;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class ApiPlugin : BasePlugin
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

        // set up HookManager
        HookManager.Initialize();

        #region a plugin would register hooks like this:

        // register hooks (using attributes)
        _systemHooksEntry = new MainEntryPoint(MyPluginInfo.PLUGIN_GUID);
        _systemHooksEntry.RegisterHooks();
        //_systemHooksEntry.RegisterHooks(System.Reflection.Assembly.GetExecutingAssembly());

        // procedurally register some hooks too
        var context = _systemHooksEntry.HookRegistryContext;
        //context.RegisterHook_System_OnUpdate_Prefix<DealDamageSystem>(MyHookWithSkip);
        context.RegisterHook_System_OnUpdate_Prefix<DealDamageSystem>(MyHook);

        #endregion
    }

    public override bool Unload()
    {
        // a plugin would unregister like this
        _systemHooksEntry.UnregisterHooks();

        // and everything after this, is part of this library
        HookManager.UnInitialize();
        PerformanceRecorderSystemPatch.UnInitialize();
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
