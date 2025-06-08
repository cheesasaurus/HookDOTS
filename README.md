# HookDOTS

HookDOTS is a modding library for hooking [Unity DOTS](https://unity.com/dots) games. By itself, it doesn't do anything.

#### Why use HookDOTS?

[Harmony](https://github.com/BepInEx/HarmonyX) is an excellent tool, but it fails at one important thing: Hooking [unmanaged systems](https://docs.unity3d.com/Packages/com.unity.entities@1.3/api/Unity.Entities.ISystem.html) in [IL2CPP](https://docs.unity3d.com/6000.1/Documentation/Manual/scripting-backends-il2cpp.html) builds.\
HookDOTS was created to solve this problem. It's capable of setting prefix and postfix hooks for OnUpdate calls, for both managed and unmanaged systems.

Other features
- Defer actions until after specific worlds have been created (e.g. for plugin bootstrapping)
- And that's it. More DOTS-specific features can be added as needed.



## Quick start

HookDOTS was originally developed for modding the game [VRising](https://playvrising.com/), but in principle should work with other Unity DOTS games built with IL2CPP.

The [API project](https://github.com/cheesasaurus/HookDOTS/tree/main/BepInExPlugins/API) is what mods integrate with, and targets BepInEx 6.

For a quick look at how this library can be used, see the [VRising thunderstore readme](https://github.com/cheesasaurus/HookDOTS/blob/main/BepInExPlugins/VRisingThunderstorePackage/README.md). (Click on the "For Plugin Developers" part with the small triangle next to it)

There is also an [Example Project](https://github.com/cheesasaurus/HookDOTS/tree/main/BepInExPlugins/ExamplePlugin).
