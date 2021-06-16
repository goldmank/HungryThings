using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.Ui
{
    public class LevelProgress : MonoBehaviour
    {
        [SerializeField] private Image _fill;
        [SerializeField] private RectTransform _bar;
        [SerializeField] private RectTransform[] _markers;

        public void SetProgress(float progress, bool immidate = false)
        {
            if (immidate)
            {
                _fill.fillAmount = progress;
                return;
            }
            _fill.DOFillAmount(progress, 0.3f);
        }

        public void HideMarkers()
        {
            foreach (var marker in _markers)
            {
                marker.gameObject.SetActive(false);
            }
        }
        
        public void ShowMarker(int index, float progress)
        {
            var width = _bar.sizeDelta.x;
            
            var rect = _markers[index];
            var pos = rect.anchoredPosition;
            pos.x = progress * width;
            rect.anchoredPosition = pos;
            rect.gameObject.SetActive(true);
        }
    }
}