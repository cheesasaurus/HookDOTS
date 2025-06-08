using System;
using System.Collections.Generic;

namespace HookDOTS.API.Attributes;

[AttributeUsage(AttributeTargets.Method)]
public class WhenCreatedWorldsContainAllAttribute : Attribute
{
    public IEnumerable<string> WorldNames;

    public WhenCreatedWorldsContainAllAttribute(params string[] worldNames)
    {
        WorldNames = worldNames;
    }

}
