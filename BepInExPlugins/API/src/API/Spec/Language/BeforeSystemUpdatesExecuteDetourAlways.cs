

using System;

namespace HookDOTS.API.Spec.Language;


public class BeforeSystemUpdatesExecuteDetourAlways : IEndpoint
{
    private Specification _spec;
    private Type _systemType;
    private Hooks.System_OnUpdate_Prefix.Hook _hook;

    internal BeforeSystemUpdatesExecuteDetourAlways(Specification spec, Type systemType, Hooks.System_OnUpdate_Prefix.Hook hook)
    {
        _spec = spec;
        _systemType = systemType;
        _hook = hook;
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
        return new Rule(_systemType, _hook);
    }

    private class Rule : IRule
    {
        private Type _systemType;
        private Hooks.System_OnUpdate_Prefix.Hook _hook;

        internal Rule(Type systemType, Hooks.System_OnUpdate_Prefix.Hook hook)
        {
            _systemType = systemType;
            _hook = hook;
        }

        void IRule.Apply(SpecificationContext context)
        {
            var options = Hooks.System_OnUpdate_Prefix.Options.Default;
            context.Registrar.RegisterHook_System_OnUpdate_Prefix(_hook, _systemType, options);
        }
    }
    
}
