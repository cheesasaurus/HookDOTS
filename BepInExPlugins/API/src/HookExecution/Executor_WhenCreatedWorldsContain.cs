using System;
using System.Collections.Generic;
using Unity.Entities;

namespace HookDOTS.HookRegistration;


internal class Executor_WhenCreatedWorldsContain
{
    private IWorldWatcher _worldWatcher;
    private SubRegistry_WhenCreatedWorldsContainAny _subRegistryAny;
    private SubRegistry_WhenCreatedWorldsContainAll _subRegistryAll;

    internal Executor_WhenCreatedWorldsContain(SubRegistry_WhenCreatedWorldsContainAny subRegistryAny, SubRegistry_WhenCreatedWorldsContainAll subRegistryAll, IWorldWatcher worldWatcher)
    {
        _subRegistryAny = subRegistryAny;
        _subRegistryAll = subRegistryAll;
        _worldWatcher = worldWatcher;
    }

    public void ExecuteAndRemoveHooks()
    {
        _worldWatcher.Update();
        ExecuteAndRemoveHooks_AllWorlds();
        ExecuteAndRemoveHooks_AnyWorlds();
    }

    private void ExecuteAndRemoveHooks_AnyWorlds()
    {
        var registryEntries = _subRegistryAny.ExtractMatchesInRegistrationOrder(_worldWatcher);
        foreach (var registryEntry in registryEntries)
        {
            try
            {
                var matchedWorlds = _worldWatcher.GetMatchingWorldsCreated(registryEntry.WorldNames);
                registryEntry.Hook.Invoke(matchedWorlds);
            }
            catch (Exception ex)
            {
                registryEntry.Log.LogError(ex);
                continue;
            }
        }
    }

    private void ExecuteAndRemoveHooks_AllWorlds()
    {
        var registryEntries = _subRegistryAll.ExtractMatchesInRegistrationOrder(_worldWatcher);
        foreach (var registryEntry in registryEntries)
        {
            try
            {
                var matchedWorlds = _worldWatcher.GetMatchingWorldsCreated(registryEntry.WorldNames);
                registryEntry.Hook.Invoke(matchedWorlds);
            }
            catch (Exception ex)
            {
                registryEntry.Log.LogError(ex);
                continue;
            }
        }
    }
}