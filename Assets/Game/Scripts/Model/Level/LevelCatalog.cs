using System.Linq;
using UnityEngine;

namespace Game.Scripts.Model.Level
{
    [CreateAssetMenu(fileName = "LevelCatalog", menuName = "Create Level Catalog", order = 0)]
    public class LevelCatalog : ScriptableObject
    {
        public LevelData[] Levels;

        public LevelData GetLevel(int index)
        {
            if (index >= 0 && index < Levels.Length)
            {
                return Levels[index];
            }

            return null;
        }

        public void SortByLevel()
        {
            var list = Levels.ToList();
            list.Sort((a, b) =>
            {
                if (a.LevelNumber > b.LevelNumber)
                {
                    return 1;
                }
                if (a.LevelNumber < b.LevelNumber)
                {
                    return -1;
                }
                return 0;
            });
            Levels = list.ToArray();
        }
    }
}