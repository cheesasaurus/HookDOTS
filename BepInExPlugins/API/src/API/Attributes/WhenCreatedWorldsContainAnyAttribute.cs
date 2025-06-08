using System;
using System.Collections.Generic;

namespace HookDOTS.API.Attributes;

[AttributeUsage(AttributeTargets.Method)]
public class WhenCreatedWorldsContainAnyAttribute : Attribute
{
    public IEnumerable<string> WorldNames;

    public WhenCreatedWorldsContainAnyAttribute(params string[] worldNames)
    {
        WorldNames = worldNames;
    }

}
