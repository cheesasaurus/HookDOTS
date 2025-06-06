using System;
using System.Linq;
using System.Reflection;
using Unity.Entities;

namespace HookDOTS.API.Hooks;

#nullable enable

public static class System_OnUpdate_Prefix
{
    unsafe public delegate bool Hook(SystemState* systemState);

    public class Options(bool onlyWhenSystemRuns = true, Throttle? throttle = null)
    {
        public bool OnlyWhenSystemRuns = onlyWhenSystemRuns;
        public Throttle? Throttle = throttle;

        public static Options Default => new Options();
    }

    public static class HookAdapter
    {
        public delegate bool HookVariant1();
        unsafe public delegate void HookVariant2(SystemState* systemState);
        public delegate void HookVariant3();

        public static Hook Adapt(MethodInfo methodInfo)
        {
            dynamic? suppliedHook = null;
            var paramCount = methodInfo.GetParameters().Length;
            var param0Info = methodInfo.GetParameters().ElementAtOrDefault(0);
            var param0Type = param0Info?.ParameterType;

            if (paramCount > 1)
            {
                // invalid signature
            }
            else if (methodInfo.ReturnType == typeof(bool))
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
            else if (methodInfo.ReturnType == typeof(void))
            {
                if (param0Type == typeof(SystemState*))
                {
                    suppliedHook = methodInfo.CreateDelegate<HookVariant2>();
                }
                else if (param0Type == null)
                {
                    suppliedHook = methodInfo.CreateDelegate<HookVariant3>();
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
                return suppliedHook();
            };
        }

        unsafe public static Hook Adapt(HookVariant2 suppliedHook)
        {
            return (systemState) =>
            {
                suppliedHook(systemState);
                return true;
            };
        }

        unsafe public static Hook Adapt(HookVariant3 suppliedHook)
        {
            return (systemState) =>
            {
                suppliedHook();
                return true;
            };
        }

    }

}