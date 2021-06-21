using System;
using UnityEngine;

namespace Game.Scripts.Model.World
{
    [Serializable]
    public class WorldData
    {
        public WorldType Type;
        public Sprite Icon;
        public Scripts.Level.World Prefab;
        public PriceType PriceType;
        public int Price;
    }
}