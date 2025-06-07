using System;

namespace HookDOTS.API.Spec.Language;


public class AfterSystemUpdatesExecuteAction
{
    private Specification _spec;
    private Type _systemType;
    private bool _onlyWhenSystemRuns;
    private Hooks.System_OnUpdate_Postfix.Hook _hook;

    internal AfterSystemUpdatesExecuteAction(Specification spec, Type systemType, bool onlyWhenSystemRuns, Hooks.System_OnUpdate_Postfix.Hook hook)
    {
        _spec = spec;
        _systemType = systemType;
        _onlyWhenSystemRuns = onlyWhenSystemRuns;
        _hook = hook;
    }

    public AfterSystemUpdatesExecuteActionAlways Always()
    {
        return new AfterSystemUpdatesExecuteActionAlways(_spec, _systemType, _onlyWhenSystemRuns, _hook);
    }

    public AfterSystemUpdatesExecuteActionThrottled Throttled(int days = 0, int hours = 0, int minutes = 0, int seconds = 0, int milliseconds = 0)
    {
        var throttleInterval = new TimeSpan(
            days: days,
            hours: hours,
            minutes: minutes,
            seconds: seconds,
            milliseconds: milliseconds
        );
        return new AfterSystemUpdatesExecuteActionThrottled(_spec, _systemType, _onlyWhenSystemRuns, _hook, throttleInterval);
    }
    
}
