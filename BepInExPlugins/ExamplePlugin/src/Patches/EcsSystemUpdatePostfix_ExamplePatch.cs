using System;
using HookDOTS.API.Attributes;
using Unity.Entities;
using ProjectM;
using ProjectM.Gameplay.Systems;

namespace ExamplePlugin.Patches;

public class EcsSystemUpdatePostfix_ExamplePatch
{
    // The hook must be `public` and `static`. If you want SystemState* passed in, an `unsafe` context is also required.
    [EcsSystemUpdatePostfix(typeof(EquipItemSystem))]
    unsafe public static void ExamplePostfix0(SystemState* systemState)
    {
        var world = systemState->World;
        ExamplePlugin.LogInstance.LogInfo($"ExamplePostfix0 executing in world {world.Name}.");
        // unlike a prefix hook, we cannot return false to skip anything. the only valid return type is `void`.
    }

    // You can leave the method parameters empty if desired.
    [EcsSystemUpdatePostfix(typeof(EquipItemSystem))]
    public static void ExamplePostfix1()
    {
        ExamplePlugin.LogInstance.LogInfo($"ExamplePostfix1 executing.");
    }

    // You can set `onlyWhenSystemRuns` to `false`, and the hook will be called even if the system didn't actually run.
    [EcsSystemUpdatePostfix(typeof(EquipItemSystem), onlyWhenSystemRuns: false)]
    public static void ExamplePostfix2()
    {
        // (this is commented out, because the log would be spammed every frame)
        // ExamplePlugin.LogInstance.LogInfo($"ExamplePostfix2 executing");
    }

    // The [Throttle] attribute can be used to rate-limit hooks that you expect to be running too frequently.
    // You can specify `days`, `hours`, `minutes`, `seconds`, and `milliseconds` to define the throttle interval.
    // Internally, these are used to create a `System.TimeSpan`
    [Throttle(seconds: 2)]
    [EcsSystemUpdatePostfix(typeof(EquipItemSystem), onlyWhenSystemRuns: false)]
    public static void ExamplePostfixThrottled()
    {
        ExamplePlugin.LogInstance.LogInfo($"ExamplePostfixThrottled executing (throttled to once every 2 seconds)");
    }

    // Unmanaged systems can of course be hooked too.
    [Throttle(seconds: 2)]
    [EcsSystemUpdatePostfix(typeof(DealDamageSystem), onlyWhenSystemRuns: false)]
    public static void ExamplePostfixThrottled2()
    {
        ExamplePlugin.LogInstance.LogInfo($"ExamplePostfixThrottled2 executing (throttled to once every 2 seconds)");
    }

    // If a hook throws an Exception during execution, an error will be logged and execution will continue.
    // Further postfix hooks will not be skipped.
    [EcsSystemUpdatePostfix(typeof(EquipItemSystem))]
    public static void ExamplePostfixThrows()
    {
        throw new Exception("OH NO! I CAN THROW! (in the postfix)");
    }

    // This is not a valid method signature. The hook will not be registered, and an error will be logged.
    [EcsSystemUpdatePostfix(typeof(EquipItemSystem))]
    public static int MyInvalidPostfix1(int a, int b)
    {
        return a + b;
    }

    // This is also not a valid method signature. The hook will not be registered, and an error will be logged.
    [EcsSystemUpdatePostfix(typeof(EquipItemSystem))]
    unsafe public static void MyInvalidPostfix2(SystemState* systemState, int c)
    {
        
    }

}