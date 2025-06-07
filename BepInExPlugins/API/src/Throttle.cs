using System;

namespace HookDOTS;

public class Throttle
{
    private TimeSpan _interval;
    private DateTime _blockUntilTime = DateTime.MinValue;

    public Throttle(TimeSpan interval)
    {
        _interval = interval;
    }

    public Throttle(int days = 0, int hours = 0, int minutes = 0, int seconds = 0, int milliseconds = 0)
    {
        _interval = new TimeSpan(
            days: days,
            hours: hours,
            minutes: minutes,
            seconds: seconds,
            milliseconds: milliseconds
        );
    }

    // return true if a throttle should be applied (to block execution)
    public bool CheckAndTrigger()
    {
        if (DateTime.Now < _blockUntilTime)
        {
            return true;
        }
        _blockUntilTime = DateTime.Now.Add(_interval);
        return false;
    }

}