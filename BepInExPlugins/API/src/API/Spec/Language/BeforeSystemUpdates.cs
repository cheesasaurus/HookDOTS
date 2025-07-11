using System;

namespace HookDOTS.API.Spec.Language;


public class BeforeSystemUpdates
{
    private Specification _spec;
    private Type _systemType;
    private bool _onlyWhenSystemRuns;

    internal BeforeSystemUpdates(Specification spec, Type systemType, bool onlyWhenSystemRuns)
    {
        _spec = spec;
        _systemType = systemType;
        _onlyWhenSystemRuns = onlyWhenSystemRuns;
    }

    // C# does not seem to have type unions. so we have ourselves a bunch of overloads

    public BeforeSystemUpdatesExecuteDetour ExecuteDetour(Hooks.System_OnUpdate_Prefix.HookFunction detour)
    {
        var hook = Hooks.System_OnUpdate_Prefix.CreateHook(detour);
        return ExecuteDetour(hook);
    }

    public BeforeSystemUpdatesExecuteDetour ExecuteDetour(Hooks.System_OnUpdate_Prefix.HookFunctionVariant1 detour)
    {
        var hook = Hooks.System_OnUpdate_Prefix.CreateHook(detour);
        return ExecuteDetour(hook);
    }

    public BeforeSystemUpdatesExecuteDetour ExecuteDetour(Hooks.System_OnUpdate_Prefix.HookFunctionVariant2 detour)
    {
        var hook = Hooks.System_OnUpdate_Prefix.CreateHook(detour);
        return ExecuteDetour(hook);
    }

    public BeforeSystemUpdatesExecuteDetour ExecuteDetour(Hooks.System_OnUpdate_Prefix.HookFunctionVariant3 detour)
    {
        var hook = Hooks.System_OnUpdate_Prefix.CreateHook(detour);
        return ExecuteDetour(hook);
    }

    public BeforeSystemUpdatesExecuteDetour ExecuteDetour(Hooks.System_OnUpdate_Prefix.Hook detour)
    {
        return new BeforeSystemUpdatesExecuteDetour(_spec, _systemType, _onlyWhenSystemRuns, detour);
    }

}
