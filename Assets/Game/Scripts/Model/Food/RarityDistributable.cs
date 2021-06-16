using Game.Scripts.Infra;

namespace Game.Scripts.Model.HiddenObject
{
    public class RarityDistributable : IDistributable
    {
        public object Value => _type;
        public int Amount => _dist;

        private ObjectRarity _type;
        private int _dist;
        
        public RarityDistributable(ObjectRarity rewardType, int dist)
        {
            _type = rewardType;
            _dist = dist;
        }
    }
}