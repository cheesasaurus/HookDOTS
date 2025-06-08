using System.Collections.Generic;

namespace HookDOTS.API.Spec.Language;


public class WhenCreatedWorldsContainAny
{
    private Specification _spec;
    private IEnumerable<string> _worldNames;

    internal WhenCreatedWorldsContainAny(Specification spec, IEnumerable<string> worldNames)
    {
        _spec = spec;
        _worldNames = worldNames;
    }

    public WhenCreatedWorldsContainAnyExecuteActionOnce ExecuteActionOnce(Hooks.WhenCreatedWorldsContainAny.HookFunction action)
    {
        var hook = Hooks.WhenCreatedWorldsContainAny.CreateHook(action);
        return new WhenCreatedWorldsContainAnyExecuteActionOnce(_spec, _worldNames, hook);
    }

    public WhenCreatedWorldsContainAnyExecuteActionOnce ExecuteActionOnce(Hooks.WhenCreatedWorldsContainAny.HookFunctionVariant1 action)
    {
        var hook = Hooks.WhenCreatedWorldsContainAny.CreateHook(action);
        return new WhenCreatedWorldsContainAnyExecuteActionOnce(_spec, _worldNames, hook);
    }

    public WhenCreatedWorldsContainAnyExecuteActionOnce ExecuteActionOnce(Hooks.WhenCreatedWorldsContainAny.Hook action)
    {
        return new WhenCreatedWorldsContainAnyExecuteActionOnce(_spec, _worldNames, action);
    }

}
