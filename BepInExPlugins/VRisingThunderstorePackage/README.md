# HookDOTS API

HookDOTS is a modding library for hooking [Unity DOTS](https://unity.com/dots) games. By itself, it doesn't do anything.

#### Why use HookDOTS?

[Harmony](https://github.com/BepInEx/HarmonyX) is an excellent tool, but it fails at one important thing: Hooking [unmanaged systems](https://docs.unity3d.com/Packages/com.unity.entities@1.3/api/Unity.Entities.ISystem.html) in [IL2CPP](https://docs.unity3d.com/6000.1/Documentation/Manual/scripting-backends-il2cpp.html) builds.\
HookDOTS was created to solve this problem.

Notable attributes provided (for plugin developers):

- `[EcsSystemUpdatePrefix(typeof(EquipItemSystem))]`
- `[EcsSystemUpdatePostfix(typeof(EquipItemSystem))]`
- `[WhenCreatedWorldsContainAny(["Server", "Client_0"])]`


## Installation

- Install [BepInEx](https://v-rising.thunderstore.io/package/BepInEx/BepInExPack_V_Rising/).
- Extract `HookDOTS.API.dll` into `(VRising folder)/BepInEx/plugins`.


## Usage

**For Server Operators**

By itself, HookDOTS doesn't do anything. It is required by other plugins and should be kept up to date.



<details><summary><strong>For Plugin Developers</strong></summary>

## How to use

#### 1. Add a reference to the plugin.
>`dotnet add package HookDOTS.API`

#### 2. Add the API plugin as a dependency via the `BepInDependency` attribute on your plugin class.
```C#
[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
[BepInDependency("HookDOTS.API")]
public class ExamplePlugin : BasePlugin
```

#### 3. Register hooks in your plugin's Load method.

```C#
public override void Load()
{
    //... other initialization code

    // Register your plugin's hooks with HookDOTS
    _hookDOTS = new HookDOTS.API.HookDOTS(MyPluginInfo.PLUGIN_GUID, Log);
    _hookDOTS.RegisterAnnotatedHooks();
}
```

#### 4. Teardown in your plugin's Unload method.
```C#
public override bool Unload()
{
    //... other unloading code

    _hookDOTS.Dispose();
    return true;
}
```

#### 5. Set up prefix and postifx hooks for System OnUpdate calls. Both managed and unmanaged systems can be hooked.
```C#
public class ExamplePatch
{
    // The hook must be `public` and `static`.
    // If you want SystemState* passed in, an `unsafe` context is also required.
    [EcsSystemUpdatePrefix(typeof(EquipItemSystem))]
    unsafe public static bool ExamplePrefix(SystemState* systemState)
    {
        var world = systemState->World;
        ExamplePlugin.LogInstance.LogInfo($"ExamplePrefix executing in world {world.Name}.");

        // You can return false, to skip the hooked OnUpdate. Other prefix hooks will still run.
        bool shouldSkipTheOriginal = true;
        return !shouldSkipTheOriginal; // this will be returned, and is false. Therefore the original will be skipped.
    }

    [EcsSystemUpdatePostfix(typeof(EquipItemSystem))]
    unsafe public static void ExamplePostfix(SystemState* systemState)
    {
        var world = systemState->World;
        ExamplePlugin.LogInstance.LogInfo($"ExamplePostfix executing in world {world.Name}.");
        // Unlike a prefix hook, a postfix hook cannot return false to skip anything.
        // The only valid return type is `void`.
    }

}
```

### onlyWhenSystemRuns
You can set `onlyWhenSystemRuns` to `false`, and the hook will be called even if the system doesn't actually run.

```C#
[EcsSystemUpdatePrefix(typeof(EquipItemSystem), onlyWhenSystemRuns: false)]
public static void ExamplePrefix2()
{
    // (this is commented out, because the log would be spammed every frame)
    // ExamplePlugin.LogInstance.LogInfo($"ExamplePrefix2 executing");
}
```


### Throttling
If you're using hooks to dump information and don't want to be flooded every frame, the `Throttle` attribute can be used.
```C#
// You can specify `days`, `hours`, `minutes`, `seconds`, and `milliseconds` to define the throttle interval.
// Internally, these are used to create a `System.TimeSpan`
[Throttle(seconds: 10)]
[EcsSystemUpdatePrefix(typeof(EquipItemSystem), onlyWhenSystemRuns: false)]
public static void ExamplePrefixThrottled()
{
    ExamplePlugin.LogInstance.LogInfo($"ExamplePrefixThrottled executing (throttled to once every 10 seconds)");
}
```


### Alternative ways to register hooks
Procedurally, using a HookRegistrar:
```C#
var hookDOTS = new HookDOTS.API.HookDOTS(MyPluginInfo.PLUGIN_GUID, Log);
//...
var hook = HookDOTS.Hooks.System_OnUpdate_Prefix.CreateHook(MyHookMethod);
hookDOTS.HookRegistrar.RegisterHook_System_OnUpdate_Prefix<TakeDamageInSunSystem_Server>(hook);
```
Builder style, using SetupHooks:
```C#
var hookDOTS = new HookDOTS.API.HookDOTS(MyPluginInfo.PLUGIN_GUID, Log);
//...
hookDOTS
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
```


### World Readiness triggers for Initialization

There are world readiness checks/triggers if you need to defer something until after worlds are created.

Worlds are immediately checked during hook registration, in case they were already set up.\
Each registered hook will only be executed once. (Even if it doesn't complete, e.g. due to errors)

```C#
// The hook must be `public` and `static`.
[WhenCreatedWorldsContainAny(["Server", "Client_0"])]
unsafe public static void ExampleInitializer1(IEnumerable<World> worlds)
{
    ExamplePlugin.LogInstance.LogInfo($"{worlds.First().Name} world is ready.");
}

// there is also an "All" version (instead of "Any")
[WhenCreatedWorldsContainAll(["Server", "Default World"])]
public static void ExampleInitializerAll()
{
    ExamplePlugin.LogInstance.LogInfo($"ExampleInitializerAll executing.");
}
```
Builder style, using SetupHooks:
```C#
hookDOTS
    .SetupHooks()
        .WhenCreatedWorldsContainAny(["Server", "Default World"])
            .ExecuteActionOnce(LogSomething)
            .And()
            .ExecuteActionOnce(DeferredInitialize)
    .RegisterChain();
    // be sure to call RegisterChain! Otherwise the entire chain will be discarded.
```



### Example project

A full [example project](https://github.com/cheesasaurus/HookDOTS/tree/main/BepInExPlugins/ExamplePlugin) is available. Of particular interest:
- [Plugin entry point](https://github.com/cheesasaurus/HookDOTS/blob/main/BepInExPlugins/ExamplePlugin/ExamplePlugin.cs)
- [Prefix hook examples](https://github.com/cheesasaurus/HookDOTS/blob/main/BepInExPlugins/ExamplePlugin/src/Patches/EcsSystemUpdatePrefix_ExamplePatch.cs)
- [Postfix hook examples](https://github.com/cheesasaurus/HookDOTS/blob/main/BepInExPlugins/ExamplePlugin/src/Patches/EcsSystemUpdatePostfix_ExamplePatch.cs)

</details>

## Support

Join the [modding community](https://vrisingmods.com/discord).

Post an issue on the [GitHub repository](https://github.com/cheesasaurus/HookDOTS). 