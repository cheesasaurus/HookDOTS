using System;
using System.Linq;
using System.Reflection;
using Unity.Entities;

namespace HookDOTS.API.Hooks;

#nullable enable

public static class System_OnUpdate_Postfix
{
    unsafe public delegate void HookFunction(SystemState* systemState);

    public class Hook(HookFunction func, MethodInfo unwrappedMethodInfo)
    {
        public HookFunction Func { get; } = func;
        public MethodInfo UnwrappedMethodInfo { get; } = unwrappedMethodInfo;

        unsafe public void Invoke(SystemState* systemState)
        {
            Func(systemState);
        }
    }
    public static Hook CreateHook(MethodInfo methodInfo)
    {
        var hookFunc = HookFunctionAdapter.Adapt(methodInfo);
        return new Hook(hookFunc, methodInfo);
    }

    public class Options(bool onlyWhenSystemRuns = true, Throttle? throttle = null)
    {
        public bool OnlyWhenSystemRuns = onlyWhenSystemRuns;
        public Throttle? Throttle = throttle;
        public static Options Default => new Options();
    }

    public static class HookFunctionAdapter
    {
        private delegate void HookVariant1();

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
                    suppliedHook = methodInfo.CreateDelegate<HookVariant1>();
                }
            }

            if (suppliedHook == null)
            {
                var declaringType = methodInfo.DeclaringType;
                throw new Exception($"Invalid hook format on {declaringType?.FullName}.{methodInfo.Name}");
            }

            return Adapt(suppliedHook);
        }

        unsafe private static HookFunction Adapt(HookFunction suppliedHook)
        {
            return suppliedHook;
        }

        unsafe private static HookFunction Adapt(HookVariant1 suppliedHook)
        {
            return (systemState) =>
            {
                suppliedHook();
            };
        }

    }

}