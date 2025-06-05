namespace HookDOTS.API.HookRegistration;


internal interface IdGenerator
{
    public int NextId();
}

internal class IdGeneratorAutoIncrement : IdGenerator
{
    private int _autoIncrement = 0;
    public int NextId()
    {
        return ++_autoIncrement;
    }
}