

using System;

namespace HookDOTS.API.Spec.Language;


public class AfterSystemUpdatesExecuteActionAlways : IEndpoint
{
    private Specification _spec;
    private Type _systemType;
    private bool _onlyWhenSystemRuns;
    private Hooks.System_OnUpdate_Postfix.Hook _hook;

    internal AfterSystemUpdatesExecuteActionAlways(Specification spec, Type systemType, bool onlyWhenSystemRuns, Hooks.System_OnUpdate_Postfix.Hook hook)
    {
        _spec = spec;
        _systemType = systemType;
        _onlyWhenSystemRuns = onlyWhenSystemRuns;
        _hook = hook;
    }

    public AfterSystemUpdates And()
    {
        AddRuleToSpec();
        return new AfterSystemUpdates(_spec, _systemType, _onlyWhenSystemRuns);
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
        return new Rule(_systemType, _onlyWhenSystemRuns, _hook);
    }

    private class Rule: IRule
    {
        private Type _systemType;
        private bool _onlyWhenSystemRuns;
        private Hooks.System_OnUpdate_Postfix.Hook _hook;

        internal Rule(Type systemType, bool onlyWhenSystemRuns, Hooks.System_OnUpdate_Postfix.Hook hook)
        {
            _systemType = systemType;
            _onlyWhenSystemRuns = onlyWhenSystemRuns;
            _hook = hook;
        }

        void IRule.Apply(SpecificationContext context)
        {
            var options = new Hooks.System_OnUpdate_Postfix.Options()
            {
                OnlyWhenSystemRuns = _onlyWhenSystemRuns,
                Throttle = null,
            };
            context.Registrar.RegisterHook_System_OnUpdate_Postfix(_hook, _systemType, options);
        }
    }
    
}
