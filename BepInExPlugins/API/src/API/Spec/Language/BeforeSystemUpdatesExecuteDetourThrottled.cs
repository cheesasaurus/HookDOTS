

using System;

namespace HookDOTS.API.Spec.Language;


public class BeforeSystemUpdatesExecuteDetourThrottled
{
    private Specification _spec;
    private Type _systemType;
    private Hooks.System_OnUpdate_Prefix.Hook _hook;
    private TimeSpan _throttleInterval;

    internal BeforeSystemUpdatesExecuteDetourThrottled(Specification spec, Type systemType, Hooks.System_OnUpdate_Prefix.Hook hook, TimeSpan throttleInterval)
    {
        _spec = spec;
        _systemType = systemType;
        _hook = hook;
        _throttleInterval = throttleInterval;
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
