using System.Collections.Generic;

namespace HookDOTS.API.Spec.Language;

internal class Specification
{
    internal SpecificationContext Context;
    private List<IRule> _rules = new();

    private List<List<IRule>> _history = new();

    internal Specification(SpecificationContext context)
    {
        Context = context;
    }

    internal Specification AddRule(IRule rule)
    {
        _rules.Add(rule);
        return this;
    }

    internal void Apply()
    {
        foreach (var rule in _rules)
        {
            rule.Apply(Context);
        }
        _history.Add(_rules);
        _rules = new();
    }

}