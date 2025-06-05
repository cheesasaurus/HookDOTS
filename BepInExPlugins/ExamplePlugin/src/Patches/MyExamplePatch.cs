using System;
using ProjectM;
using ProjectM.Gameplay.Systems;
using HookDOTS.API.Attributes;
using VRisingMods.Core.Utilities;
using Unity.Entities;

namespace ExamplePlugin.Patches;

public class MyExamplePatch
{
    private static TimeSpan twoSeconds = new TimeSpan(hours: 0, minutes: 0, seconds: 2);
    private static DateTime nextTime = DateTime.MinValue;

    [EcsSystemUpdatePrefix(typeof(StatChangeSystem), onlyWhenSystemRuns: true)]
    unsafe public static bool ExamplePrefix(SystemState* systemState) //note: must be public
    {
        // return false, to skip further prefixes and the original
        bool shouldSkipFurtherPrefixesAndTheOriginal = false; // this is false
        bool returnVal = !shouldSkipFurtherPrefixesAndTheOriginal; // this will be returned, and is true. Therefore no skip.

        if (DateTime.Now < nextTime)
        {
            return returnVal;
        }
        nextTime = DateTime.Now.Add(twoSeconds);
        LogUtil.LogInfo($"[{DateTime.Now}] ExamplePrefix executing. (debounce 2 seconds)");
        return returnVal;
    }

    // todo: what if we want to return void
    // todo: what if we don't want parameters
    // todo: Can we create a wrapper to hold the variations?
    // todo: handling for invalid method signatures

}