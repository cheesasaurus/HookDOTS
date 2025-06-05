using System;
using System.Reflection;
using HookDOTS.API.Attributes;
using HookDOTS.API.Hooks;

namespace HookDOTS.API;

//
// Summary:
//     The HookDOTS instance is the main entry to HookDOTS. After creating one with a
//     unique identifier, it is used to patch and query the current application domain
public class HookDOTS
{
    public string Id { get; }

    //
    // Summary:
    //     The HookRegistrar can be used to procedurally register hooks
    public HookRegistrar HookRegistrar { get; }

    //
    // Summary:
    //     Creates a new HookDOTS instance
    //
    // Parameters:
    //   id:
    //     A unique identifier (you choose your own)
    //
    // Returns:
    //     A HookDOTS instance
    public HookDOTS(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            throw new ArgumentException("id cannot be null or empty");
        }
        Id = id;
        HookRegistrar = HookManager.NewHookRegistrar(id);
    }

    public void RegisterHooks()
    {
        RegisterHooks(Assembly.GetCallingAssembly());
    }

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
        // todo: register postfix
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
        var hook = methodInfo.CreateDelegate<System_OnUpdate_Prefix.Hook>();
        var options = new System_OnUpdate_Prefix.Options(onlyWhenSystemRuns: attribute.OnlyWhenSystemRuns);
        HookRegistrar.RegisterHook_System_OnUpdate_Prefix(hook, attribute.SystemType, options);
        var declaringType = methodInfo.DeclaringType;
        // LogUtil.LogDebug($"registered EcsSystemUpdatePrefix hook: {declaringType.FullName}.{methodInfo.Name}");
        return true;
    }

}