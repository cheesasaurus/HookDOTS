using System.Collections.Generic;
using HookDOTS.HookRegistration;

namespace HookDOTS.API.Spec.Language;

internal class SpecificationContext
{
    public HookRegistrar Registrar;

    internal SpecificationContext(HookRegistrar registrar)
    {
        Registrar = registrar;
    }
    
}