using Unity.Entities;

namespace HookDOTS.API.Hooks;


unsafe public delegate void Hook_System_OnUpdate_Postfix(SystemState* systemState);

public struct HookOptions_System_OnUpdate_Postfix(bool onlyWhenSystemRuns = true)
{
    public bool OnlyWhenSystemRuns = onlyWhenSystemRuns;
    public static HookOptions_System_OnUpdate_Postfix Default => new HookOptions_System_OnUpdate_Postfix();
}