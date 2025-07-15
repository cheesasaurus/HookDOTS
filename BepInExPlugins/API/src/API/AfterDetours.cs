namespace HookDOTS.API;

/// <summary>
/// We are mimicking the Harmony API,
/// which allows detours to return false to skip the original method.
/// This is a bit of a magic value. AfterDetours attempts to bring clarity with named constants.
/// </summary>
public static class AfterDetours
{
    /// <summary>
    /// Return value indicating that the original method should be skipped.
    /// </summary>
    public const bool SkipOriginalMethod = false;

    /// <summary>
    /// Return value indicating that's it's ok to run the original method.
    /// The method could still be skipped due to another detour's result.
    /// </summary>
    public const bool OkToRunOriginalMethod = true;
    
}