using System;
using Il2CppInterop.Runtime;
using HookDOTS.API.Hooks;
using BepInEx.Logging;


namespace HookDOTS.API.HookRegistration;

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
    

}