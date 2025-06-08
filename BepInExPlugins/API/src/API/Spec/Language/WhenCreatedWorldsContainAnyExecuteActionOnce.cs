using System.Collections.Generic;

namespace HookDOTS.API.Spec.Language;


public class WhenCreatedWorldsContainAnyExecuteActionOnce
{
    private Specification _spec;
    private IEnumerable<string> _worldNames;
    private Hooks.WhenCreatedWorldsContainAny.Hook _hook;

    internal WhenCreatedWorldsContainAnyExecuteActionOnce(Specification spec, IEnumerable<string> worldNames, Hooks.WhenCreatedWorldsContainAny.Hook hook)
    {
        _spec = spec;
        _worldNames = worldNames;
        _hook = hook;
    }

    public WhenCreatedWorldsContainAny And()
    {
        AddRuleToSpec();
        return new WhenCreatedWorldsContainAny(_spec, _worldNames);
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
        return new Rule(_worldNames, _hook);
    }

    private class Rule: IRule
    {
        private IEnumerable<string> _worldNames;
        private Hooks.WhenCreatedWorldsContainAny.Hook _hook;


        internal Rule(IEnumerable<string> worldNames, Hooks.WhenCreatedWorldsContainAny.Hook hook)
        {
            _worldNames = worldNames;
            _hook = hook;
        }

        void IRule.Apply(SpecificationContext context)
        {
            context.Registrar.RegisterHook_WhenCreatedWorldsContainAny(_hook, _worldNames);
        }
    }

}
