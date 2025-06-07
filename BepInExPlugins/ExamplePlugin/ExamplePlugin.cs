using System;
using System.Collections.Generic;
using System.Linq;
using BepInEx;
using BepInEx.Logging;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using HookDOTS;
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
        _hookDOTS.RegisterAnnotatedHooks();

        // It's possible to procedurally register hooks
        RegisterHooks_Procedurally();

        // Also possible to register hooks with a builder-like syntax
        RegisterHooks_BuilderStyle();
    }

    unsafe private void RegisterHooks_Procedurally()
    {
        var hook1 = HookDOTS.Hooks.System_OnUpdate_Prefix.CreateHook(MyProcedurallyRegisteredHook);
        _hookDOTS.HookRegistrar.RegisterHook_System_OnUpdate_Prefix<TakeDamageInSunSystem_Server>(hook1);

        var hook2 = HookDOTS.Hooks.System_OnUpdate_Prefix.CreateHook(MyHookWithPartialSignature);
        _hookDOTS.HookRegistrar.RegisterHook_System_OnUpdate_Prefix<TakeDamageInSunSystem_Server>(hook2);

        var hook3 = HookDOTS.Hooks.WhenCreatedWorldsContainAny.CreateHook(LogFirstGivenWorld);
        _hookDOTS.HookRegistrar.RegisterHook_WhenCreatedWorldsContainAny(hook3, ["Bilbo", "Default World", "Server"]);

        var hook4 = HookDOTS.Hooks.WhenCreatedWorldsContainAll.CreateHook(LogGivenWorlds);
        _hookDOTS.HookRegistrar.RegisterHook_WhenCreatedWorldsContainAll(hook4, ["Default World", "Server"]);
    }

    unsafe private void RegisterHooks_BuilderStyle()
    {
        _hookDOTS
            .SetupHooks()
                .BeforeSystemUpdates<EquipItemSystem>()
                    .ExecuteDetour(MyMethodA).Always()
                    .And()
                    .ExecuteDetour(MyMethodB).Always()
            .Also()
                .AfterSystemUpdates<TakeDamageInSunSystem_Server>()
                    .ExecuteAction(MyMethodC).Throttled(seconds: 2)
            .Also()
                .BeforeSystemUpdates<DropInventoryItemSystem>(onlyWhenSystemRuns: false)
                    .ExecuteDetour(MyMethodD).Throttled(seconds: 5)
            .RegisterChain();
        // be sure to call RegisterChain! Otherwise the entire chain will be discarded.
    }

    public override bool Unload()
    {
        // be sure to call Dispose! This will unregister hooks and clean up to avoid memory leaks.
        _hookDOTS.Dispose();

        // Harmony unpatching
        _harmony?.UnpatchSelf();
        return true;
    }

    private Throttle throttle1 = new Throttle(seconds: 2);
    unsafe private bool MyProcedurallyRegisteredHook(SystemState* systemState)
    {
        if (throttle1.CheckAndTrigger())
        {
            return false;
        }
        Log.LogInfo($"MyProcedurallyRegisteredHook executing. (limit once per 2 seconds)");
        return false;
    }

    private Throttle throttle2 = new Throttle(seconds: 2);
    private void MyHookWithPartialSignature()
    {
        if (throttle2.CheckAndTrigger())
        {
            return;
        }
        Log.LogInfo($"MyHookWithPartialSignature executing. (limit once per 2 seconds)");
        return;
    }

    private void MyMethodA()
    {
        Log.LogInfo($"MyMethodA executing.");
    }

    private void MyMethodB()
    {
        Log.LogInfo($"MyMethodB executing.");
    }

    private void MyMethodC()
    {
        Log.LogInfo($"MyMethodC executing.");
    }

    private void MyMethodD()
    {
        Log.LogInfo($"MyMethodD executing.");
    }

    private void LogFirstGivenWorld(IEnumerable<World> worlds)
    {
        Log.LogWarning($"First world: {worlds.First().Name}");
    }

    private void LogGivenWorlds(IEnumerable<World> worlds)
    {
        var worldNames = worlds.Select(w => w.Name);
        var worldNamesString = string.Join(", ", worldNames);
        Log.LogInfo($"All worlds: {worldNamesString}");
    }

}
