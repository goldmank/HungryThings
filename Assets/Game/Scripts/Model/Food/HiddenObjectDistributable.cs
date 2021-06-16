using Game.Scripts.Infra;
using Game.Scripts.Model.Food;

namespace Game.Scripts.Model.HiddenObject
{
    public class HiddenObjectDistributable : IDistributable
    {
        public object Value => _type;
        public int Amount => _dist;

        private FoodType _type;
        private int _dist;
        
        public HiddenObjectDistributable(FoodType type, ObjectRarity rarity)
        {
            _type = type;
            _dist = GetRarityDist(rarity);
        }
        
        private int GetRarityDist(ObjectRarity r)
        {
            switch (r)
            {
                case ObjectRarity.Common: return 100;
                case ObjectRarity.Rare: return 50;
                case ObjectRarity.Epic: return 25;
                case ObjectRarity.Legendary: return 10;
            }

            return 1;
        }
    }
}