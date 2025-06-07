using System;
using Il2CppInterop.Runtime;
using HookDOTS.Hooks;
using BepInEx.Logging;
using System.Collections.Generic;
using System.Linq;


namespace HookDOTS.HookRegistration;

//
// Summary:
//     A HookRegistrar instance provides a public way to register and unregister hooks.
public class HookRegistrar
{
    private string _id;
    private HookRegistryStaging _hookRegistryStaging;
    private ManualLogSource _log;

    internal HookRegistrar(string registrarId, HookRegistryStaging hookRegistryStaging, ManualLogSource log)
    {
        _id = registrarId;
        _hookRegistryStaging = hookRegistryStaging;
        _log = log;
    }

    public void Dispose()
    {
        _hookRegistryStaging.Dispose();
    }

    public void UnregisterHooks()
    {
        _hookRegistryStaging.CancelPendingRegistrations();
        _hookRegistryStaging.UnregisterRegisteredHooks();
    }

    ////////////////////////////////////////////////////////////////////

    #region Hook Registration: System_OnUpdate_Prefix

    public void RegisterHook_System_OnUpdate_Prefix<TSystemType>(System_OnUpdate_Prefix.Hook hook)
    {
        var options = System_OnUpdate_Prefix.Options.Default;
        RegisterHook_System_OnUpdate_Prefix<TSystemType>(hook, options);
    }

    public void RegisterHook_System_OnUpdate_Prefix<TSystemType>(System_OnUpdate_Prefix.Hook hook, System_OnUpdate_Prefix.Options options)
    {
        RegisterHook_System_OnUpdate_Prefix(hook, Il2CppType.Of<TSystemType>(), options);
    }

    public void RegisterHook_System_OnUpdate_Prefix(System_OnUpdate_Prefix.Hook hook, Type systemType)
    {
        var options = System_OnUpdate_Prefix.Options.Default;
        RegisterHook_System_OnUpdate_Prefix(hook, systemType, options);
    }

    public void RegisterHook_System_OnUpdate_Prefix(System_OnUpdate_Prefix.Hook hook, Type systemType, System_OnUpdate_Prefix.Options options)
    {
        RegisterHook_System_OnUpdate_Prefix(hook, Il2CppType.From(systemType), options);
    }

    public void RegisterHook_System_OnUpdate_Prefix(System_OnUpdate_Prefix.Hook hook, Il2CppSystem.Type systemType)
    {
        var options = System_OnUpdate_Prefix.Options.Default;
        RegisterHook_System_OnUpdate_Prefix(hook, systemType, options);
    }

    public void RegisterHook_System_OnUpdate_Prefix(System_OnUpdate_Prefix.Hook hook, Il2CppSystem.Type systemType, System_OnUpdate_Prefix.Options options)
    {
        _hookRegistryStaging.RegisterHook_System_OnUpdate_Prefix(hook, systemType, options);
    }

    #endregion

    ////////////////////////////////////////////////////////////////////

    #region Hook Registration: System_OnUpdate_Postfix

    public void RegisterHook_System_OnUpdate_Postfix<TSystemType>(System_OnUpdate_Postfix.Hook hook)
    {
        var options = System_OnUpdate_Postfix.Options.Default;
        RegisterHook_System_OnUpdate_Postfix<TSystemType>(hook, options);
    }

    public void RegisterHook_System_OnUpdate_Postfix<TSystemType>(System_OnUpdate_Postfix.Hook hook, System_OnUpdate_Postfix.Options options)
    {
        RegisterHook_System_OnUpdate_Postfix(hook, Il2CppType.Of<TSystemType>(), options);
    }

    public void RegisterHook_System_OnUpdate_Postfix(System_OnUpdate_Postfix.Hook hook, Type systemType)
    {
        var options = System_OnUpdate_Postfix.Options.Default;
        RegisterHook_System_OnUpdate_Postfix(hook, systemType, options);
    }

    public void RegisterHook_System_OnUpdate_Postfix(System_OnUpdate_Postfix.Hook hook, Type systemType, System_OnUpdate_Postfix.Options options)
    {
        RegisterHook_System_OnUpdate_Postfix(hook, Il2CppType.From(systemType), options);
    }

    public void RegisterHook_System_OnUpdate_Postfix(System_OnUpdate_Postfix.Hook hook, Il2CppSystem.Type systemType)
    {
        var options = System_OnUpdate_Postfix.Options.Default;
        RegisterHook_System_OnUpdate_Postfix(hook, systemType, options);
    }

    public void RegisterHook_System_OnUpdate_Postfix(System_OnUpdate_Postfix.Hook hook, Il2CppSystem.Type systemType, System_OnUpdate_Postfix.Options options)
    {
        _hookRegistryStaging.RegisterHook_System_OnUpdate_Postfix(hook, systemType, options);
    }

    #endregion

    ////////////////////////////////////////////////////////////////////

    #region Hook Registration: WhenCreatedWorldsContainAny

    public void RegisterHook_WhenCreatedWorldsContainAny(WhenCreatedWorldsContainAny.Hook hook, IEnumerable<string> worldNames)
    {
        if (!worldNames.Any())
        {
            throw new ArgumentOutOfRangeException("worldNames cannot be empty");
        }
        _hookRegistryStaging.RegisterHook_WhenCreatedWorldsContainAny(hook, worldNames.ToHashSet());
    }

    #endregion

    ////////////////////////////////////////////////////////////////////

    #region Hook Registration: WhenCreatedWorldsContainAll

    public void RegisterHook_WhenCreatedWorldsContainAll(WhenCreatedWorldsContainAll.Hook hook, IEnumerable<string> worldNames)
    {
        if (!worldNames.Any())
        {
            throw new ArgumentException("worldNames cannot be empty");
        }
        _hookRegistryStaging.RegisterHook_WhenCreatedWorldsContainAll(hook, worldNames.ToHashSet());
    }

    #endregion

    ////////////////////////////////////////////////////////////////////


}