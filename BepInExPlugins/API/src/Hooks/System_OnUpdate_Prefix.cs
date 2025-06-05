using Unity.Entities;

namespace HookDOTS.API.Hooks;


public static class System_OnUpdate_Prefix
{
    unsafe public delegate bool Func(SystemState* systemState);

    public struct Options(bool onlyWhenSystemRuns = true)
    {
        public bool OnlyWhenSystemRuns = onlyWhenSystemRuns;
        public static Options Default => new Options();
    }

}