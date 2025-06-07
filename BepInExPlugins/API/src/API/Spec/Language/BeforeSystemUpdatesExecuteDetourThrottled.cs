

using System;

namespace HookDOTS.API.Spec.Language;


public class BeforeSystemUpdatesExecuteDetourThrottled : IEndpoint
{
    private Specification _spec;
    private Type _systemType;
    private Hooks.System_OnUpdate_Prefix.Hook _hook;
    private TimeSpan _throttleInterval;

    internal BeforeSystemUpdatesExecuteDetourThrottled(Specification spec, Type systemType, Hooks.System_OnUpdate_Prefix.Hook hook, TimeSpan throttleInterval)
    {
        _spec = spec;
        _systemType = systemType;
        _hook = hook;
        _throttleInterval = throttleInterval;
    }

    public BeforeSystemUpdates And()
    {
        AddRuleToSpec();
        return new BeforeSystemUpdates(_spec, _systemType);
    }

    public SetupHooks Also()
    {
        AddRuleToSpec();
        return new SetupHooks(_spec);
    }

    public void RegisterChain()
    {
        AddRuleToSpec();
        _spec.Apply();
    }

    private void AddRuleToSpec()
    {
        _spec.AddRule(CreateRule());
    }

    private IRule CreateRule()
    {
        return new Rule(_systemType, _hook, _throttleInterval);
    }

    private class Rule: IRule
    {
        private Type _systemType;
        private Hooks.System_OnUpdate_Prefix.Hook _hook;
        private TimeSpan _throttleInterval;

        internal Rule(Type systemType, Hooks.System_OnUpdate_Prefix.Hook hook, TimeSpan throttleInterval)
        {
            _systemType = systemType;
            _hook = hook;
            _throttleInterval = throttleInterval;
        }

        void IRule.Apply(SpecificationContext context)
        {
            var options = new Hooks.System_OnUpdate_Prefix.Options()
            {
                OnlyWhenSystemRuns = true,
                Throttle = new Throttle(_throttleInterval)
            };
            context.Registrar.RegisterHook_System_OnUpdate_Prefix(_hook, _systemType, options);
        }
    }
    
    
}
