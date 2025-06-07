namespace HookDOTS;

public delegate void GameReadyForHookingHandler();

public class Bus
{
    public static readonly Bus Instance = new();

    public event GameReadyForHookingHandler GameReadyForHooking;

    public void TriggerGameReadyForHooking()
    {
        GameReadyForHooking?.Invoke();
    }
    
}
