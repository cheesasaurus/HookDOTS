using System;

namespace HookDOTS.API.Spec.Language;


public class AfterSystemUpdates
{
    private Specification _spec;
    private Type _systemType;

    internal AfterSystemUpdates(Specification spec, Type systemType)
    {
        _spec = spec;
        _systemType = systemType;
    }

    public AfterSystemUpdatesExecuteAction ExecuteAction(Hooks.System_OnUpdate_Postfix.HookFunction action)
    {
        var hook = Hooks.System_OnUpdate_Postfix.CreateHook(action);
        return new AfterSystemUpdatesExecuteAction(_spec, _systemType, hook);
    }

    public AfterSystemUpdatesExecuteAction ExecuteAction(Hooks.System_OnUpdate_Postfix.HookFunctionVariant1 action)
    {
        var hook = Hooks.System_OnUpdate_Postfix.CreateHook(action);
        return new AfterSystemUpdatesExecuteAction(_spec, _systemType, hook);
    }

    public AfterSystemUpdatesExecuteAction ExecuteAction(Hooks.System_OnUpdate_Postfix.Hook action)
    {
        return new AfterSystemUpdatesExecuteAction(_spec, _systemType, action);
    }

}
