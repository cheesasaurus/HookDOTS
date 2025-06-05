using System;

namespace ExamplePlugin;

public class Throttler
{
    private TimeSpan _interval;
    private DateTime _blockUntilTime = DateTime.MinValue;

    public Throttler(TimeSpan interval)
    {
        _interval = interval;
    }

    public T RunIfNotBlocked<T>(Func<T> func, T defaultReturnVal)
    {
        if (DateTime.Now < _blockUntilTime)
        {
            return defaultReturnVal;
        }
        _blockUntilTime = DateTime.Now.Add(_interval);
        return func();
    }

}