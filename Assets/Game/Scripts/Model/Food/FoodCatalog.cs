using System.Collections.Generic;
using Game.Scripts.Infra;
using Game.Scripts.Model.HiddenObject;
using UnityEngine;

namespace Game.Scripts.Model.Food
{
    [CreateAssetMenu(fileName = "FoodCatalog", menuName = "Create Food Catalog", order = 0)]
    public class FoodCatalog : ScriptableObject
    {
        private const int HistorySize = 4;
        
        public FoodData[] Objects;
        public Color[] RarityColor;
        public Material[] RarityMaterials;

        private List<FoodType> _spawnHistory = new List<FoodType>();
        
        public FoodData GetFood(FoodType type)
        {
            foreach (var foodData in Objects)
            {
                if (foodData.Type == type)
                {
                    return foodData;
                }
            }

            return null;
        }
        
        public FoodType GetRandomHiddenObjectType()
        {
            // var dist = new List<IDistributable>();
            // foreach (var hiddenObjectData in Objects)
            // {
            //     if (ModelManager.Get().GlobalPref.ObjectsUnlockCount < 1 && hiddenObjectData.Rarity != ObjectRarity.Common)
            //     {
            //         continue;
            //     }
            //     dist.Add(new HiddenObjectDistributable(hiddenObjectData.Type, hiddenObjectData.Rarity));
            // }
            // var result = ValueDistributor.GetRandomValue<FoodType>(dist.ToArray());
            //
            // while (_spawnHistory.Contains(result))
            // {
            //     result = ValueDistributor.GetRandomValue<FoodType>(dist.ToArray());    
            // }
            // _spawnHistory.Add(result);
            // if (_spawnHistory.Count > HistorySize)
            // {
            //     _spawnHistory.RemoveAt(0);
            // }
            //
            // return result;
            return FoodType.Cat;
        }

        public void InitRarityMaterials()
        {
            for (var i = 0; i < RarityMaterials.Length; i++)
            {
                RarityMaterials[i].SetColor("_ColorX", RarityColor[i]);
            }
        }
    }
}