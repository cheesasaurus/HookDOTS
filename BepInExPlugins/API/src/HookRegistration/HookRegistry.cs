

namespace HookDOTS.API.HookRegistration;


internal class HookRegistry
{
    internal SubRegistry_System_OnUpdate_Prefix SubRegistry_System_OnUpdate_Prefix { get; }
    private IdGenerator _idGenerator;

    internal HookRegistry()
    {
        _idGenerator = new IdGeneratorAutoIncrement();
        SubRegistry_System_OnUpdate_Prefix = new(_idGenerator);
    }

    internal void UnregisterHook(HookHandle hookHandle)
    {
        switch (hookHandle.HookType)
        {
            case HookType.System_OnUpdate_Prefix:
                SubRegistry_System_OnUpdate_Prefix.UnregisterHook(hookHandle);
                break;
            case HookType.System_OnUpdate_Postfix:
                // todo. would need to add a way to register first
                break;
        }
    }

}
