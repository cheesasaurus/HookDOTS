using System;
using HookDOTS.API.Attributes;
using Unity.Entities;
using ProjectM;

namespace ExamplePlugin.Patches;

public class EcsSystemUpdatePrefix_ExamplePatch
{
    // The hook must be `public` and `static`. If you want SystemState* passed in, an `unsafe` context is also required.
    [EcsSystemUpdatePrefix(typeof(EquipItemSystem))]
    unsafe public static bool ExamplePrefix0(SystemState* systemState)
    {
        var world = systemState->World;
        ExamplePlugin.LogInstance.LogInfo($"ExamplePrefix0 executing in world {world.Name}.");

        // You can return false, to skip further prefix hooks and the original OnUpdate call
        bool shouldSkipFurtherPrefixesAndTheOriginal = false; // this is false
        return !shouldSkipFurtherPrefixesAndTheOriginal; // this will be returned, and is true. Therefore no skip.
    }

    // You can leave the method parameters empty if desired.
    [EcsSystemUpdatePrefix(typeof(EquipItemSystem))]
    public static bool ExamplePrefix1()
    {
        ExamplePlugin.LogInstance.LogInfo($"ExamplePrefix1 executing.");
        return true;
    }

    // You don't have to return a bool. `void` is permitted.
    [EcsSystemUpdatePrefix(typeof(EquipItemSystem))]
    unsafe public static void ExamplePrefix2(SystemState* systemState)
    {
        var world = systemState->World;
        ExamplePlugin.LogInstance.LogInfo($"ExamplePrefix2 executing in world {world.Name}.");
    }

    // It's also fine to return `void` and have no parameters.
    [EcsSystemUpdatePrefix(typeof(EquipItemSystem))]
    public static void ExamplePrefix3()
    {
        ExamplePlugin.LogInstance.LogInfo($"ExamplePrefix3 executing.");
    }

    // You can set `onlyWhenSystemRuns` to `false`, and the hook will be called even if the system doesn't actually run.
    // Note that if another similar hook returns false, this hook can still get skipped.
    [EcsSystemUpdatePrefix(typeof(EquipItemSystem), onlyWhenSystemRuns: false)]
    public static void ExamplePrefix4()
    {
        // (this is commented out, because the log would be spammed every frame)
        // ExamplePlugin.LogInstance.LogInfo($"ExamplePrefix4 executing");
    }

    // The [Throttle] attribute can be used to rate-limit hooks that you expect to be running too frequently.
    // You can specify `days`, `hours`, `minutes`, `seconds`, and `milliseconds` to define the throttle interval.
    // Internally, these are used to create a `System.TimeSpan`
    [Throttle(seconds: 2)]
    [EcsSystemUpdatePrefix(typeof(EquipItemSystem), onlyWhenSystemRuns: false)]
    public static void ExamplePrefixThrottled()
    {
        ExamplePlugin.LogInstance.LogInfo($"ExamplePrefixThrottled executing (throttled to once every 2 seconds)");
    }

    // If a hook throws an Exception during execution, an error will be logged and execution will continue.
    // Further prefix hooks will not be skipped and neither will the original OnUpdate call.
    [EcsSystemUpdatePrefix(typeof(EquipItemSystem))]
    public static bool ExamplePrefixThrows()
    {
        throw new Exception("OH NO! I CAN THROW! (in the prefix)");
    }

    // This is not a valid method signature. The hook will not be registered, and an error will be logged.
    [EcsSystemUpdatePrefix(typeof(EquipItemSystem))]
    public static int MyInvalidPrefix1(int a, int b)
    {
        return a + b;
    }

    // This is also not a valid method signature. The hook will not be registered, and an error will be logged.
    [EcsSystemUpdatePrefix(typeof(EquipItemSystem))]
    unsafe public static void MyInvalidPrefix2(SystemState* systemState, int c)
    {
        
    }

}