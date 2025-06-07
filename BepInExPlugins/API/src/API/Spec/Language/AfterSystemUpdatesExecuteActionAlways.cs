

using System;

namespace HookDOTS.API.Spec.Language;


public class AfterSystemUpdatesExecuteActionAlways
{
    private Specification _spec;
    private Type _systemType;
    private Hooks.System_OnUpdate_Postfix.Hook _hook;

    internal AfterSystemUpdatesExecuteActionAlways(Specification spec, Type systemType, Hooks.System_OnUpdate_Postfix.Hook hook)
    {
        _spec = spec;
        _systemType = systemType;
        _hook = hook;
    }

    public AfterSystemUpdates And()
    {
        AddRuleToSpec();
        return new AfterSystemUpdates(_spec, _systemType);
    }

    public SetupHooks Also()
    {
        AddRuleToSpec();
        return new SetupHooks(_spec);
    }

    public SetupHooks Apply()
    {
        AddRuleToSpec();
        _spec.Apply();
        return new SetupHooks(_spec);
    }

    private void AddRuleToSpec()
    {
        // todo: implement
    }
    
}
