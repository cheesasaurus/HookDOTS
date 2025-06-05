using System;

namespace HookDOTS.API.Attributes;

[AttributeUsage(AttributeTargets.Method)]
public class EcsSystemUpdatePostfixAttribute : Attribute
{
    public Type SystemType { get; }
    public bool OnlyWhenSystemRuns { get; }

    public EcsSystemUpdatePostfixAttribute(Type systemType, bool onlyWhenSystemRuns = true)
    {
        SystemType = systemType;
        OnlyWhenSystemRuns = onlyWhenSystemRuns;
    }

}
