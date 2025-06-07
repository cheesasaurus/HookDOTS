using System;
using Il2CppInterop.Runtime;
using HookDOTS.API.Hooks;
using BepInEx.Logging;


namespace HookDOTS.API.HookRegistration;

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

    public void RegisterHook_System_OnUpdate_Prefix<TSystemType>(System_OnUpdate_Prefix.HookSignature hook)
    {
        var options = System_OnUpdate_Prefix.Options.Default;
        RegisterHook_System_OnUpdate_Prefix<TSystemType>(hook, options);
    }

    public void RegisterHook_System_OnUpdate_Prefix<TSystemType>(System_OnUpdate_Prefix.HookSignature hook, System_OnUpdate_Prefix.Options options)
    {
        RegisterHook_System_OnUpdate_Prefix(hook, Il2CppType.Of<TSystemType>(), options);
    }

    public void RegisterHook_System_OnUpdate_Prefix(System_OnUpdate_Prefix.HookSignature hook, Type systemType)
    {
        var options = System_OnUpdate_Prefix.Options.Default;
        RegisterHook_System_OnUpdate_Prefix(hook, systemType, options);
    }

    public void RegisterHook_System_OnUpdate_Prefix(System_OnUpdate_Prefix.HookSignature hook, Type systemType, System_OnUpdate_Prefix.Options options)
    {
        RegisterHook_System_OnUpdate_Prefix(hook, Il2CppType.From(systemType), options);
    }

    public void RegisterHook_System_OnUpdate_Prefix(System_OnUpdate_Prefix.HookSignature hook, Il2CppSystem.Type systemType)
    {
        var options = System_OnUpdate_Prefix.Options.Default;
        RegisterHook_System_OnUpdate_Prefix(hook, systemType, options);
    }

    public void RegisterHook_System_OnUpdate_Prefix(System_OnUpdate_Prefix.HookSignature hook, Il2CppSystem.Type systemType, System_OnUpdate_Prefix.Options options)
    {
        _hookRegistryStaging.RegisterHook_System_OnUpdate_Prefix(hook, systemType, options);
    }

    #endregion

    ////////////////////////////////////////////////////////////////////

    #region Hook Registration: System_OnUpdate_Postfix

    public void RegisterHook_System_OnUpdate_Postfix<TSystemType>(System_OnUpdate_Postfix.HookSignature hook)
    {
        var options = System_OnUpdate_Postfix.Options.Default;
        RegisterHook_System_OnUpdate_Postfix<TSystemType>(hook, options);
    }

    public void RegisterHook_System_OnUpdate_Postfix<TSystemType>(System_OnUpdate_Postfix.HookSignature hook, System_OnUpdate_Postfix.Options options)
    {
        RegisterHook_System_OnUpdate_Postfix(hook, Il2CppType.Of<TSystemType>(), options);
    }

    public void RegisterHook_System_OnUpdate_Postfix(System_OnUpdate_Postfix.HookSignature hook, Type systemType)
    {
        var options = System_OnUpdate_Postfix.Options.Default;
        RegisterHook_System_OnUpdate_Postfix(hook, systemType, options);
    }

    public void RegisterHook_System_OnUpdate_Postfix(System_OnUpdate_Postfix.HookSignature hook, Type systemType, System_OnUpdate_Postfix.Options options)
    {
        RegisterHook_System_OnUpdate_Postfix(hook, Il2CppType.From(systemType), options);
    }

    public void RegisterHook_System_OnUpdate_Postfix(System_OnUpdate_Postfix.HookSignature hook, Il2CppSystem.Type systemType)
    {
        var options = System_OnUpdate_Postfix.Options.Default;
        RegisterHook_System_OnUpdate_Postfix(hook, systemType, options);
    }

    public void RegisterHook_System_OnUpdate_Postfix(System_OnUpdate_Postfix.HookSignature hook, Il2CppSystem.Type systemType, System_OnUpdate_Postfix.Options options)
    {
        _hookRegistryStaging.RegisterHook_System_OnUpdate_Postfix(hook, systemType, options);
    }

    #endregion

    ////////////////////////////////////////////////////////////////////


}