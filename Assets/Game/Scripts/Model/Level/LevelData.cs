using System;
using Game.Scripts.Infra;
using Game.Scripts.Model.Food;
using UnityEngine;

namespace Game.Scripts.Model.Level
{
    [Serializable]
    public class LevelData
    {
        public int LevelNumber;
        public LevelType Type;
        public float CameraY;
        public Material Background;
        public float ChanceForCoin;
        public RangeFloat CoinReward;
        public float[] Markers;
        public int Duration;
        public GameObject SpawnObject;
        public Sprite SpawnObjectIcon;
        public int SpawnedObjectsCount;
        public FoodType Food { get; set; }
    }
}