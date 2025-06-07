

namespace HookDOTS.API.Spec.Language;


public class After
{
    private Specification _spec;

    internal After(Specification spec)
    {
        _spec = spec;
    }

    public AfterSystemUpdates SystemUpdates<TSystemType>()
    {
        return new AfterSystemUpdates(_spec, typeof(TSystemType));
    }

}
