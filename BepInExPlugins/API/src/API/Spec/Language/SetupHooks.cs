

using System.Collections.Generic;

namespace HookDOTS.API.Spec.Language;


public class SetupHooks
{
    private Specification _spec;

    internal SetupHooks(Specification spec)
    {
        _spec = spec;
    }

    public BeforeSystemUpdates BeforeSystemUpdates<TSystemType>(bool onlyWhenSystemRuns = true)
    {
        return new BeforeSystemUpdates(_spec, typeof(TSystemType), onlyWhenSystemRuns);
    }

    public AfterSystemUpdates AfterSystemUpdates<TSystemType>(bool onlyWhenSystemRuns = true)
    {
        return new AfterSystemUpdates(_spec, typeof(TSystemType), onlyWhenSystemRuns);
    }

    public WhenCreatedWorldsContainAny WhenCreatedWorldsContainAny(IEnumerable<string> worldNames)
    {
        return new WhenCreatedWorldsContainAny(_spec, worldNames);
    }

    public WhenCreatedWorldsContainAll WhenCreatedWorldsContainAll(IEnumerable<string> worldNames)
    {
        return new WhenCreatedWorldsContainAll(_spec, worldNames);
    }
    
}
