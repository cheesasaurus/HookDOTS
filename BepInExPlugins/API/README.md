# HookDOTS.API

todo: thorough API documentation. for now, refer to the [VRising documentation](https://github.com/cheesasaurus/HookDOTS/blob/main/BepInExPlugins/VRisingBootstrapper/README.md).


## Bootstrapper plugin

If using the API with a different game, you will need to create a bootstrapper plugin.

The bootstrapper's sole responsibility is to signal to the API when the game is ready for hooking.\
Hooked systems need to be identifiable by Unity's [TypeManager](https://docs.unity3d.com/Packages/com.unity.entities@1.3/api/Unity.Entities.TypeManager.html).

Registering hooks initially places them in a kind of "staging area", and they won't actually be aplied until the API gets the signal from the bootstrapper.

Sending the signal is a one-liner:
```C#
HookDOTS.API.Bus.Instance.TriggerGameReadyForHooking();
```