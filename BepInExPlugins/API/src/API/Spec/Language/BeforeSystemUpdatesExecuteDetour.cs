using System;

namespace HookDOTS.API.Spec.Language;


public class BeforeSystemUpdatesExecuteDetour
{
    private Specification _spec;
    private Type _systemType;
    private Hooks.System_OnUpdate_Prefix.Hook _hook;

    internal BeforeSystemUpdatesExecuteDetour(Specification spec, Type systemType, Hooks.System_OnUpdate_Prefix.Hook hook)
    {
        _spec = spec;
        _systemType = systemType;
        _hook = hook;
    }

    public BeforeSystemUpdatesExecuteDetourAlways Always()
    {
        return new BeforeSystemUpdatesExecuteDetourAlways(_spec, _systemType, _hook);
    }

    public BeforeSystemUpdatesExecuteDetourThrottled Throttled(int days = 0, int hours = 0, int minutes = 0, int seconds = 0, int milliseconds = 0)
    {
        var throttleInterval = new TimeSpan(
            days: days,
            hours: hours,
            minutes: minutes,
            seconds: seconds,
            milliseconds: milliseconds
        );
        return new BeforeSystemUpdatesExecuteDetourThrottled(_spec, _systemType, _hook, throttleInterval);
    }

}
