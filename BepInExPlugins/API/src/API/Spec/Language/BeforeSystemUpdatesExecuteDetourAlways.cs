

using System;

namespace HookDOTS.API.Spec.Language;


public class BeforeSystemUpdatesExecuteDetourAlways
{
    private Specification _spec;
    private Type _systemType;
    private Hooks.System_OnUpdate_Prefix.Hook _hook;

    internal BeforeSystemUpdatesExecuteDetourAlways(Specification spec, Type systemType, Hooks.System_OnUpdate_Prefix.Hook hook)
    {
        _spec = spec;
        _systemType = systemType;
        _hook = hook;
    }

    public BeforeSystemUpdates And()
    {
        AddRuleToSpec();
        return new BeforeSystemUpdates(_spec, _systemType);
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
