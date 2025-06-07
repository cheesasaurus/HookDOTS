using System;
using System.Linq;
using System.Reflection;
using Unity.Entities;

namespace HookDOTS.API.Hooks;

#nullable enable

public static class System_OnUpdate_Postfix
{
    unsafe public delegate void HookSignature(SystemState* systemState);

    public class Options(bool onlyWhenSystemRuns = true, Throttle? throttle = null)
    {
        public bool OnlyWhenSystemRuns = onlyWhenSystemRuns;
        public Throttle? Throttle = throttle;
        public static Options Default => new Options();
    }

    public static class HookAdapter
    {
        private delegate void HookVariant1();

        public static HookSignature Adapt(MethodInfo methodInfo)
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
                    suppliedHook = methodInfo.CreateDelegate<HookSignature>();
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

        unsafe private static HookSignature Adapt(HookSignature suppliedHook)
        {
            return suppliedHook;
        }

        unsafe private static HookSignature Adapt(HookVariant1 suppliedHook)
        {
            return (systemState) =>
            {
                suppliedHook();
            };
        }

    }

}