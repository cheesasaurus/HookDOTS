using System;
using System.Collections.Generic;

namespace HookDOTS.API.Attributes;

[AttributeUsage(AttributeTargets.Method)]
public class WhenCreatedWorldsContainAllAttribute : Attribute
{
    public IEnumerable<string> WorldNames;

    public WhenCreatedWorldsContainAllAttribute(string[] worldNames)
    {
        WorldNames = worldNames;
    }

}
