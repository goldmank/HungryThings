using System;
using Game.Scripts.Model.HiddenObject;
using UnityEngine;

namespace Game.Scripts.Model.Food
{
    [Serializable]
    public class FoodData
    {
        public FoodType Type;
        public string Name;
        public ObjectRarity Rarity;
        public Sprite Icon;
        public FoodObject Prefab;
    }
}