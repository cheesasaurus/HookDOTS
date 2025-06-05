using Unity.Entities;

namespace HookDOTS.API.Hooks;


unsafe public delegate bool Hook_System_OnUpdate_Prefix(SystemState* systemState);

public struct HookOptions_System_OnUpdate_Prefix(bool onlyWhenSystemRuns = true)
{
    public bool OnlyWhenSystemRuns = onlyWhenSystemRuns;
    public static HookOptions_System_OnUpdate_Prefix Default => new HookOptions_System_OnUpdate_Prefix();
}