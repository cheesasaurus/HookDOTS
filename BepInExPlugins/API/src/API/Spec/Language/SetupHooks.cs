

namespace HookDOTS.API.Spec.Language;


public class SetupHooks
{
    private Specification _spec;

    internal SetupHooks(Specification spec)
    {
        _spec = spec;
    }

    public BeforeSystemUpdates BeforeSystemUpdates<TSystemType>()
    {
        return new BeforeSystemUpdates(_spec, typeof(TSystemType));
    }

    public AfterSystemUpdates AfterSystemUpdates<TSystemType>()
    {
        return new AfterSystemUpdates(_spec, typeof(TSystemType));
    }
    
}
