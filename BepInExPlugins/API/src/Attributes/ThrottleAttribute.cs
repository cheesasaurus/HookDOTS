using System;

namespace HookDOTS.API.Attributes;

[AttributeUsage(AttributeTargets.Method)]
public class ThrottleAttribute : Attribute
{
    public TimeSpan Interval;

    public ThrottleAttribute(int days = 0, int hours = 0, int minutes = 0, int seconds = 0, int milliseconds = 0)
    {
        Interval = new TimeSpan(
            days: days,
            hours: hours,
            minutes: minutes,
            seconds: seconds,
            milliseconds: milliseconds
        );
    }

    public Throttle CreateThrottle()
    {
        return new Throttle(Interval);
    }

}
