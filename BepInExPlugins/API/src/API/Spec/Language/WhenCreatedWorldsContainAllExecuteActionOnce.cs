using System.Collections.Generic;

namespace HookDOTS.API.Spec.Language;


public class WhenCreatedWorldsContainAllExecuteActionOnce : IEndpoint
{
    private Specification _spec;
    private IEnumerable<string> _worldNames;
    private Hooks.WhenCreatedWorldsContainAll.Hook _hook;

    internal WhenCreatedWorldsContainAllExecuteActionOnce(Specification spec, IEnumerable<string> worldNames, Hooks.WhenCreatedWorldsContainAll.Hook hook)
    {
        _spec = spec;
        _worldNames = worldNames;
        _hook = hook;
    }

    public WhenCreatedWorldsContainAll And()
    {
        AddRuleToSpec();
        return new WhenCreatedWorldsContainAll(_spec, _worldNames);
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
        private Hooks.WhenCreatedWorldsContainAll.Hook _hook;


        internal Rule(IEnumerable<string> worldNames, Hooks.WhenCreatedWorldsContainAll.Hook hook)
        {
            _worldNames = worldNames;
            _hook = hook;
        }

        void IRule.Apply(SpecificationContext context)
        {
            context.Registrar.RegisterHook_WhenCreatedWorldsContainAll(_hook, _worldNames);
        }
    }

}
