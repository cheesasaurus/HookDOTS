using System;
using System.Linq;
using System.Reflection;
using Unity.Entities;

namespace HookDOTS.API.Hooks;

#nullable enable

public static class System_OnUpdate_Postfix
{
    unsafe public delegate void Hook(SystemState* systemState);

    public class Options(bool onlyWhenSystemRuns = true, Throttle? throttle = null)
    {
        public bool OnlyWhenSystemRuns = onlyWhenSystemRuns;
        public Throttle? Throttle = throttle;
        public static Options Default => new Options();
    }

    public static class HookAdapter
    {
        public delegate void HookVariant1();

        public static Hook Adapt(MethodInfo methodInfo)
        {
            dynamic? suppliedHook = null;
            var param0Info = methodInfo.GetParameters().ElementAtOrDefault(0);
            var param0Type = param0Info?.ParameterType;
            if (methodInfo.ReturnType == typeof(void))
            {
                if (param0Type == typeof(SystemState*))
                {
                    suppliedHook = methodInfo.CreateDelegate<Hook>();
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

        unsafe public static Hook Adapt(Hook suppliedHook)
        {
            return suppliedHook;
        }

        unsafe public static Hook Adapt(HookVariant1 suppliedHook)
        {
            return (systemState) =>
            {
                suppliedHook();
            };
        }

    }

}