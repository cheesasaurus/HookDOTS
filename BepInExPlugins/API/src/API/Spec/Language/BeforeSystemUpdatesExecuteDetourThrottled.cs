

using System;

namespace HookDOTS.API.Spec.Language;


public class BeforeSystemUpdatesExecuteDetourThrottled : IEndpoint
{
    private Specification _spec;
    private Type _systemType;
    private bool _onlyWhenSystemRuns;
    private Hooks.System_OnUpdate_Prefix.Hook _hook;
    private TimeSpan _throttleInterval;

    internal BeforeSystemUpdatesExecuteDetourThrottled(Specification spec, Type systemType, bool onlyWhenSystemRuns, Hooks.System_OnUpdate_Prefix.Hook hook, TimeSpan throttleInterval)
    {
        _spec = spec;
        _systemType = systemType;
        _onlyWhenSystemRuns = onlyWhenSystemRuns;
        _hook = hook;
        _throttleInterval = throttleInterval;
    }

    public BeforeSystemUpdates And()
    {
        AddRuleToSpec();
        return new BeforeSystemUpdates(_spec, _systemType, _onlyWhenSystemRuns);
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
        return new Rule(_systemType, _onlyWhenSystemRuns, _hook, _throttleInterval);
    }

    private class Rule: IRule
    {
        private Type _systemType;
        private bool _onlyWhenSystemRuns;
        private Hooks.System_OnUpdate_Prefix.Hook _hook;
        private TimeSpan _throttleInterval;

        internal Rule(Type systemType, bool onlyWhenSystemRuns, Hooks.System_OnUpdate_Prefix.Hook hook, TimeSpan throttleInterval)
        {
            _systemType = systemType;
            _onlyWhenSystemRuns = onlyWhenSystemRuns;
            _hook = hook;
            _throttleInterval = throttleInterval;
        }

        void IRule.Apply(SpecificationContext context)
        {
            var options = new Hooks.System_OnUpdate_Prefix.Options()
            {
                OnlyWhenSystemRuns = _onlyWhenSystemRuns,
                Throttle = new Throttle(_throttleInterval)
            };
            context.Registrar.RegisterHook_System_OnUpdate_Prefix(_hook, _systemType, options);
        }
    }
    
    
}
