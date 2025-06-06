namespace HookDOTS.API;

public delegate void GameReadyForRegistrationHandler();

public class Bus
{
    public static readonly Bus Instance = new();

    public event GameReadyForRegistrationHandler GameReadyForRegistration;

    public void TriggerGameReadyForRegistration()
    {
        GameReadyForRegistration?.Invoke();
    }
    
}
