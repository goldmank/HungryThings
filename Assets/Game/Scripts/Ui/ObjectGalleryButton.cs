using Game.Scripts.Model;
using Game.Scripts.Model.HiddenObject;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.Ui
{
    public class ObjectGalleryButton : GalleryButton
    {
        [SerializeField] private Image _rarity;
        
        public void SetRarity(ObjectRarity rarity)
        {
            _rarity.color = ModelManager.Get().Foods.RarityColor[(int) rarity];
            var rarityMaterial = ModelManager.Get().Foods.RarityMaterials[(int) rarity];
            SetHiddenColor(rarityMaterial);
        }
    }
}