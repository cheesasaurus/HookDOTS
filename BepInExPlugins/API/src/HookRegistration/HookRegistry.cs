

namespace HookDOTS.HookRegistration;


internal class HookRegistry
{
    internal SubRegistry_System_OnUpdate_Prefix SubRegistry_System_OnUpdate_Prefix { get; }
    internal SubRegistry_System_OnUpdate_Postfix SubRegistry_System_OnUpdate_Postfix { get; }
    internal SubRegistry_WhenCreatedWorldsContainAny SubRegistry_WhenCreatedWorldsContainAny { get; }
    internal SubRegistry_WhenCreatedWorldsContainAll SubRegistry_WhenCreatedWorldsContainAll { get; }

    private IdGenerator _idGenerator;

    internal HookRegistry()
    {
        _idGenerator = new IdGeneratorAutoIncrement();
        SubRegistry_System_OnUpdate_Prefix = new(_idGenerator);
        SubRegistry_System_OnUpdate_Postfix = new(_idGenerator);
        SubRegistry_WhenCreatedWorldsContainAny = new(_idGenerator);
        SubRegistry_WhenCreatedWorldsContainAll = new(_idGenerator);
    }

    internal void UnregisterHook(HookHandle hookHandle)
    {
        switch (hookHandle.HookType)
        {
            case HookType.System_OnUpdate_Prefix:
                SubRegistry_System_OnUpdate_Prefix.UnregisterHook(hookHandle);
                break;
            case HookType.System_OnUpdate_Postfix:
                SubRegistry_System_OnUpdate_Postfix.UnregisterHook(hookHandle);
                break;
            case HookType.WhenCreatedWorldsContainAny:
                SubRegistry_WhenCreatedWorldsContainAny.UnregisterHook(hookHandle);
                break;
            case HookType.WhenCreatedWorldsContainAll:
                SubRegistry_WhenCreatedWorldsContainAll.UnregisterHook(hookHandle);
                break;
        }
    }

}
