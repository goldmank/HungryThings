using UnityEngine;

namespace Game.Scripts.Model.World
{
    [CreateAssetMenu(fileName = "WorldCatalog", menuName = "Create World Catalog", order = 0)]
    public class WorldCatalog : ScriptableObject
    {
        public WorldData[] Worlds;

        public WorldData GetWorld(WorldType type)
        {
            foreach (var worldData in Worlds)
            {
                if (worldData.Type == type)
                {
                    return worldData;
                }
            }

            return null;
        }
    }
}