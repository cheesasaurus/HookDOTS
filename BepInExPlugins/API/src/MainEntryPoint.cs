using System;
using System.Reflection;
using HookDOTS.API.Attributes;
using HookDOTS.API.Hooks;

namespace HookDOTS.API;

public class MainEntryPoint
{
    public string Id { get; }
    public HookRegistrar HookRegistrar { get; }

    public MainEntryPoint(string id)
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
        if (!methodInfo.IsStatic) {
            return false;
        }
        var attribute = methodInfo.GetCustomAttribute<EcsSystemUpdatePrefixAttribute>();
        if (attribute is null)
        {
            return false;
        }
        var hook = methodInfo.CreateDelegate<Hook_System_OnUpdate_Prefix>();
        var options = new HookOptions_System_OnUpdate_Prefix(onlyWhenSystemRuns: attribute.OnlyWhenSystemRuns);
        HookRegistrar.RegisterHook_System_OnUpdate_Prefix(hook, attribute.SystemType, options);
        var declaringType = methodInfo.DeclaringType;
        // LogUtil.LogDebug($"registered EcsSystemUpdatePrefix hook: {declaringType.FullName}.{methodInfo.Name}");
        return true;
    }

}