using System;
using HookDOTS.API.Attributes;
using Unity.Entities;
using ProjectM;

namespace ExamplePlugin.Patches;

public class EcsSystemUpdatePostfix_ExamplePatch
{
    private static TimeSpan twoSeconds = new TimeSpan(hours: 0, minutes: 0, seconds: 2);
    private static Throttler _throttler1 = new Throttler(twoSeconds);

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
        // ExamplePlugin.LogInstance.LogInfo($"ExamplePrefix4 executing");
    }

    // This is not a valid method signature. The hook will not be registered, and an error will be logged.
    [EcsSystemUpdatePostfix(typeof(EquipItemSystem))]
    public static int MyInvalidPostfix(int a, int b)
    {
        return a + b;
    }

    // If a hook throws an Exception during execution, an error will be logged and execution will continue.
    // Further postfix hooks will not be skipped.
    [EcsSystemUpdatePostfix(typeof(EquipItemSystem))]
    public static void ExamplePostfixThrows()
    {
        throw new Exception("OH NO! I CAN THROW! (in the postfix)");
    }

}