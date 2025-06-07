

using System.Collections.Generic;
using System.Linq;
using Unity.Entities;

namespace HookDOTS;


interface IWorldWatcher
{
    public void Update();
    public bool AreAllWorldsCreated(ISet<string> worldNames);
    public bool AreAnyWorldsCreated(ISet<string> worldNames);
    public IEnumerable<World> GetMatchingWorldsCreated(ISet<string> worldNames);
}


internal class WorldWatcher : IWorldWatcher
{
    private HashSet<string> _createdWorldNames = new();
    private Dictionary<string, World> _createdWorlds = new();

    public void Update()
    {
        foreach (var world in World.All)
        {
            if (world.IsCreated)
            {
                _createdWorldNames.Add(world.Name);
                _createdWorlds.TryAdd(world.Name, world);
            }
        }
    }

    public bool AreAllWorldsCreated(ISet<string> worldNames)
    {
        return _createdWorldNames.IsSupersetOf(worldNames);
    }

    public bool AreAnyWorldsCreated(ISet<string> worldNames)
    {
        return _createdWorldNames.Overlaps(worldNames);
    }

    public IEnumerable<World> GetMatchingWorldsCreated(ISet<string> worldNames)
    {
        return worldNames
            .Where(_createdWorlds.ContainsKey)
            .Select(worldName => _createdWorlds[worldName]);
    }

}
