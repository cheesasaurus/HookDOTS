using HookDOTS.API.Attributes;
using Unity.Entities;
using System.Collections.Generic;
using System.Linq;

namespace ExamplePlugin.Patches;

public class WorldReadiness_ExamplePatch
{
    // The hook must be `public` and `static`.
    [WhenCreatedWorldsContainAny("Server", "Client_0")]
    unsafe public static void ExampleInitializer1(IEnumerable<World> worlds)
    {
        ExamplePlugin.LogInstance.LogError($"ExampleInitializer1 executing because \"{worlds.First().Name}\" world is ready.");
    }

    // You can leave the METHOD parameters empty if desired.
    [WhenCreatedWorldsContainAny("Server")]
    public static void ExampleInitializer2()
    {
        ExamplePlugin.LogInstance.LogInfo($"ExampleInitializer2 executing.");
    }

    // The list of world names must NOT be empty.
    [WhenCreatedWorldsContainAny([])]
    public static void ExampleInitializerInvalidAttribute()
    {
        // this will never execute, because an error will be thrown during hook registration
        ExamplePlugin.LogInstance.LogInfo($"ExampleInitializerInvalidAttribute executing.");
    }

    // there is also an "All" version (instead of "Any")
    [WhenCreatedWorldsContainAll("Server", "Default World")]
    public static void ExampleInitializerAll()
    {
        ExamplePlugin.LogInstance.LogInfo($"ExampleInitializerAll executing.");
    }

}