using System.Collections.Generic;

namespace HookDOTS.API.Spec.Language;

internal class Specification
{
    internal SpecificationContext Context;
    private List<IRule> _rules = new();

    internal Specification(SpecificationContext context)
    {
        Context = context;
    }

    internal Specification Add(IRule rule)
    {
        _rules.Add(rule);
        return this;
    }

    internal void Apply()
    {
        foreach (var rule in _rules)
        {
            rule.Apply();
        }
    }

}