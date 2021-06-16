using Game.Scripts.Level;
using UnityEngine;

namespace Game.Scripts.Model.Eater
{
    [CreateAssetMenu(fileName = "EaterCatalog", menuName = "Create Eater Catalog", order = 0)]
    public class EaterCatalog : ScriptableObject
    {
        public EaterData[] Eaters;
        
        public EaterData GetEater(EaterType type)
        {
            foreach (var eaterData in Eaters)
            {
                if (eaterData.Type == type)
                {
                    return eaterData;
                }
            }

            return null;
        }
    }
}