using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.Ui
{
    public class FindAllPopup : TimeUpPopup
    {
        [SerializeField] private Image _icon;

        public void SetIcon(Sprite icon)
        {
            _icon.sprite = icon;
        }
    }
}