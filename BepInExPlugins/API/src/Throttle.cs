using System;

namespace HookDOTS.API;

public class Throttle
{
    private TimeSpan _interval;
    private DateTime _blockUntilTime = DateTime.MinValue;

    public Throttle(TimeSpan interval)
    {
        _interval = interval;
    }

    // return true if a throttle should be applied
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