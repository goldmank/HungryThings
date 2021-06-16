using System;

namespace Game.Scripts.Infra
{
    [Serializable]
    public class RangeFloat
    {
        public float Min;
        public float Max;

        public RangeFloat(float min, float max)
        {
            Min = min;
            Max = max;
        }
    }
}