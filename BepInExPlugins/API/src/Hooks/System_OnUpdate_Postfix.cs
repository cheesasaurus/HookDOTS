using System;
using System.Linq;
using System.Reflection;
using Unity.Entities;

namespace HookDOTS.Hooks;

#nullable enable

public static class System_OnUpdate_Postfix
{
    unsafe public delegate void HookFunction(SystemState* systemState);
    public delegate void HookFunctionVariant1();

    public class Hook(HookFunction func, MethodInfo unwrappedMethodInfo)
    {
        public HookFunction Func { get; } = func;
        public MethodInfo UnwrappedMethodInfo { get; } = unwrappedMethodInfo;

        unsafe public void Invoke(SystemState* systemState)
        {
            Func(systemState);
        }

        public string FullName()
        {
            var declarerName = UnwrappedMethodInfo?.DeclaringType?.FullName;
            var methodName = UnwrappedMethodInfo?.Name;
            return $"{declarerName}.{methodName}";
        }
    }

    public static Hook CreateHook(MethodInfo methodInfo)
    {
        var hookFunc = HookFunctionAdapter.Adapt(methodInfo);
        return new Hook(hookFunc, methodInfo);
    }

    public static Hook CreateHook(HookFunction hookFunc)
    {
        return new Hook(hookFunc, hookFunc.Method);
    }

    public static Hook CreateHook(HookFunctionVariant1 hookFunc)
    {
        var adaptedHook = HookFunctionAdapter.Adapt(hookFunc);
        return new Hook(adaptedHook, hookFunc.Method);
    }

    public class Options(bool onlyWhenSystemRuns = true, Throttle? throttle = null)
    {
        public bool OnlyWhenSystemRuns = onlyWhenSystemRuns;
        public Throttle? Throttle = throttle;
        public static Options Default => new Options();
    }

    internal static class HookFunctionAdapter
    {
        public static HookFunction Adapt(MethodInfo methodInfo)
        {
            dynamic? suppliedHook = null;
            var paramCount = methodInfo.GetParameters().Length;
            var param0Info = methodInfo.GetParameters().ElementAtOrDefault(0);
            var param0Type = param0Info?.ParameterType;

            if (paramCount > 1)
            {
                // invalid signature
            }
            else if (methodInfo.ReturnType == typeof(void))
            {
                if (param0Type == typeof(SystemState*))
                {
                    suppliedHook = methodInfo.CreateDelegate<HookFunction>();
                }
                else if (param0Type == null)
                {
                    suppliedHook = methodInfo.CreateDelegate<HookFunctionVariant1>();
                }
            }

            if (suppliedHook == null)
            {
                var declaringType = methodInfo.DeclaringType;
                throw new Exception($"Invalid hook format on {declaringType?.FullName}.{methodInfo.Name}");
            }

            return Adapt(suppliedHook);
        }

        unsafe internal static HookFunction Adapt(HookFunction suppliedHook)
        {
            return suppliedHook;
        }

        unsafe internal static HookFunction Adapt(HookFunctionVariant1 suppliedHook)
        {
            return (systemState) =>
            {
                suppliedHook();
            };
        }

    }

}