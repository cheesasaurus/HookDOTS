using System;

namespace HookDOTS;


public class Bus
{
    public static readonly Bus Instance = new();

    public event Action EventGameReadyForHooking;
    public event Action EventWorldsMayHaveChanged;
    public event Action CommandRunHooks_WorldsCreated;

    public void TriggerEventGameReadyForHooking()
    {
        EventGameReadyForHooking?.Invoke();
    }

    public void TriggerEventWorldsMayHaveChanged()
    {
        EventWorldsMayHaveChanged?.Invoke();
    }
    
    public void TriggerCommandRunHooks_WorldsCreated()
    {
        CommandRunHooks_WorldsCreated?.Invoke();
    }
    
}
