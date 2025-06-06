using System;
using System.Reflection;
using BepInEx.Logging;
using HookDOTS.API.Attributes;
using HookDOTS.API.HookRegistration;
using HookDOTS.API.Hooks;

namespace HookDOTS.API;

//
// Summary:
//     A HookDOTS instance is the main entrypoint for a plugin to use HookDOTS.
//     After creating one with a unique identifier,
//     it is used to patch and query the current application domain
public class HookDOTS
{
    public string Id { get; }
    private ManualLogSource _log;

    //
    // Summary:
    //     The HookRegistrar can be used by a plugin to procedurally register hooks
    public HookRegistrar HookRegistrar { get; }

    //
    // Summary:
    //     Creates a new HookDOTS instance
    //
    // Parameters:
    //   id:
    //     A unique identifier (you choose your own)
    //   log:
    //     The log to log to (e.g. when there is an error)
    //
    // Returns:
    //     A HookDOTS instance
    public HookDOTS(string id, ManualLogSource log)
    {
        if (string.IsNullOrEmpty(id))
        {
            throw new ArgumentException("id cannot be null or empty");
        }
        Id = id;
        _log = log;
        HookRegistrar = HookManager.NewHookRegistrar(id, _log);
    }

    //
    // Summary:
    //     Dispose should be called by a plugin when the plugin unloads.
    //     This will cleanup everything set up by this instance of HookDOTS,
    //     and help avoid memory leaks.
    public void Dispose()
    {
        HookRegistrar.Dispose();
    }

    //
    // Summary:
    //     RegisterHooks can be called by a plugin to scan its assembly,
    //     and register any hooks annotated by attributes such as
    //     [EcsSystemUpdatePrefix] and [EcsSystemUpdatePostfix]
    public void RegisterHooks()
    {
        RegisterHooks(Assembly.GetCallingAssembly());
    }

    //
    // Summary:
    //     UnRegisterHooks can be called by a plugin to unregister
    //     any hooks that were registered via this instance of HookDOTS.
    public void UnregisterHooks()
    {
        HookRegistrar.UnregisterHooks();
    }

    public void RegisterHooks(Assembly assembly)
    {
        foreach (var type in assembly.GetTypes())
        {
            RegisterHooks(type);
        }
    }

    internal void RegisterHooks(Type type)
    {
        foreach (var methodInfo in type.GetMethods())
        {
            RegisterHooks(methodInfo);
        }
    }

    internal void RegisterHooks(MethodInfo methodInfo)
    {
        RegisterEcsSystemUpdatePrefix(methodInfo);
        RegisterEcsSystemUpdatePostfix(methodInfo);
    }

    internal bool RegisterEcsSystemUpdatePrefix(MethodInfo methodInfo)
    {
        if (!methodInfo.IsStatic)
        {
            return false;
        }
        var attribute = methodInfo.GetCustomAttribute<EcsSystemUpdatePrefixAttribute>();
        if (attribute is null)
        {
            return false;
        }

        try
        {
            var hook = System_OnUpdate_Prefix.HookAdapter.Adapt(methodInfo);
            var options = new System_OnUpdate_Prefix.Options(
                onlyWhenSystemRuns: attribute.OnlyWhenSystemRuns,
                throttle: methodInfo.GetCustomAttribute<ThrottleAttribute>()?.CreateThrottle()
            );
            HookRegistrar.RegisterHook_System_OnUpdate_Prefix(hook, attribute.SystemType, options);
            return true;
        }
        catch (Exception ex)
        {
            _log.LogError(ex);
            return false;
        }
    }

    internal bool RegisterEcsSystemUpdatePostfix(MethodInfo methodInfo)
    {
        if (!methodInfo.IsStatic)
        {
            return false;
        }
        var attribute = methodInfo.GetCustomAttribute<EcsSystemUpdatePostfixAttribute>();
        if (attribute is null)
        {
            return false;
        }

        try
        {
            var hook = System_OnUpdate_Postfix.HookAdapter.Adapt(methodInfo);
            var options = new System_OnUpdate_Postfix.Options(
                onlyWhenSystemRuns: attribute.OnlyWhenSystemRuns,
                throttle: methodInfo.GetCustomAttribute<ThrottleAttribute>()?.CreateThrottle()
            );
            HookRegistrar.RegisterHook_System_OnUpdate_Postfix(hook, attribute.SystemType, options);
            return true;
        }
        catch (Exception ex)
        {
            _log.LogError(ex);
            return false;
        }
    }

}