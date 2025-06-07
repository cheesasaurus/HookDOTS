

namespace HookDOTS.API.Spec.Language;


public class SetupHooks
{
    private Specification _spec;

    internal SetupHooks(Specification spec)
    {
       _spec = spec;
    }

    public After After()
    {
        return new After(_spec);
    }

    public Before Before()
    {
        return new Before(_spec);
    }
}
