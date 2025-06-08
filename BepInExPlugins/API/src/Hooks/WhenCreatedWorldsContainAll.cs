using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity.Entities;

namespace HookDOTS.Hooks;

public static class WhenCreatedWorldsContainAll
{
    public delegate void HookFunction(IEnumerable<World> worlds);
    public delegate void HookFunctionVariant1();

    // todo: extract common things to AbstractHook if possible
    public class Hook(HookFunction func, MethodInfo unwrappedMethodInfo)
    {
        public HookFunction Func { get; } = func;
        public MethodInfo UnwrappedMethodInfo { get; } = unwrappedMethodInfo;

        unsafe public void Invoke(IEnumerable<World> worlds)
        {
            Func(worlds);
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

    // can this be extracted? currently the same as the adapter in WhenCreatedWorldsContainAny
    // But they would likely change for different reasons, so leaving as is for now
    internal static class HookFunctionAdapter
    {
        public static HookFunction Adapt(MethodInfo methodInfo)
        {
            dynamic suppliedHook = null;
            var paramCount = methodInfo.GetParameters().Length;
            var param0Info = methodInfo.GetParameters().ElementAtOrDefault(0);
            var param0Type = param0Info?.ParameterType;

            if (paramCount > 1)
            {
                // invalid signature
            }
            else if (methodInfo.ReturnType == typeof(void))
            {
                if (param0Type?.IsAssignableFrom(typeof(IEnumerable<World>)) ?? false)
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
            return (worlds) =>
            {
                suppliedHook();
            };
        }

    }

}
