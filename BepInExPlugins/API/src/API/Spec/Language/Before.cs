

namespace HookDOTS.API.Spec.Language;


public class Before
{
    private Specification _spec;

    internal Before(Specification spec)
    {
        _spec = spec;
    }

    public BeforeSystemUpdates SystemUpdates<TSystemType>()
    {
        return new BeforeSystemUpdates(_spec, typeof(TSystemType));
    }
}
