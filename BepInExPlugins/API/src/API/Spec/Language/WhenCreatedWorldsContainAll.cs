using System.Collections.Generic;

namespace HookDOTS.API.Spec.Language;


public class WhenCreatedWorldsContainAll
{
    private Specification _spec;
    private IEnumerable<string> _worldNames;

    internal WhenCreatedWorldsContainAll(Specification spec, IEnumerable<string> worldNames)
    {
        _spec = spec;
        _worldNames = worldNames;
    }

    public WhenCreatedWorldsContainAllExecuteActionOnce ExecuteActionOnce(Hooks.WhenCreatedWorldsContainAll.HookFunction action)
    {
        var hook = Hooks.WhenCreatedWorldsContainAll.CreateHook(action);
        return new WhenCreatedWorldsContainAllExecuteActionOnce(_spec, _worldNames, hook);
    }

    public WhenCreatedWorldsContainAllExecuteActionOnce ExecuteActionOnce(Hooks.WhenCreatedWorldsContainAll.HookFunctionVariant1 action)
    {
        var hook = Hooks.WhenCreatedWorldsContainAll.CreateHook(action);
        return new WhenCreatedWorldsContainAllExecuteActionOnce(_spec, _worldNames, hook);
    }

    public WhenCreatedWorldsContainAllExecuteActionOnce ExecuteActionOnce(Hooks.WhenCreatedWorldsContainAll.Hook action)
    {
        return new WhenCreatedWorldsContainAllExecuteActionOnce(_spec, _worldNames, action);
    }

}
