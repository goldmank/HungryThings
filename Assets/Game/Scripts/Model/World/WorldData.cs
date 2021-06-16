using System;
using UnityEngine;

namespace Game.Scripts.Model.World
{
    [Serializable]
    public class WorldData
    {
        public WorldType Type;
        public Sprite Icon;
        public GameObject Prefab;
        public PriceType PriceType;
        public int Price;
    }
}