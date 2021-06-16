using DG.Tweening;
using Game.Scripts.Infra;
using Game.Scripts.Model;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.Ui
{
    public class TimeUpPopup : MonoBehaviour
    {
        [SerializeField] private Image _bg;
        [SerializeField] private Image _bar;
        [SerializeField] private MaskableGraphic[] _barItems;
        [SerializeField] private Button _button;

        public void Show(float showDuration = 0.2f, float stayDuration = 0)
        {
            gameObject.SetActive(true);

            _bar.fillAmount = 0;
            Utils.SetAlpha(_bar, 1);
            foreach (var maskableGraphic in _barItems)
            {
                Utils.SetAlpha(maskableGraphic, 0);
            }

            if (null != _button)
            {
                _button.gameObject.SetActive(false);    
            }
            
            Utils.SetAlpha(_bg, 0);
            _bg.DOFade(0.8f, showDuration * 0.57f).OnComplete(() => {
                _bar.DOFillAmount(1, showDuration * 0.43f).OnComplete(() =>
                {
                    foreach (var maskableGraphic in _barItems)
                    {
                        maskableGraphic.DOFade(1, 0.1f);
                    }

                    if (_button != null)
                    {
                        ModelManager.Get().Tasker.Run(() =>
                        {
                            _button.gameObject.SetActive(true);
                        }, 0.2f);   
                    }

                    if (stayDuration > 0)
                    {
                        ModelManager.Get().Tasker.Run(Hide, stayDuration);   
                    }
                }); 
            });
        }

        public void OnTryAgain()
        {
            GameManager.Get().LoadLevel();
        }

        private void Hide()
        {
            foreach (var maskableGraphic in _barItems)
            {
                maskableGraphic.DOFade(0, 0.1f);
            }

            _bar.DOFade(0, 0.2f);
            _bg.DOFade(0, 0.3f).OnComplete(() => { gameObject.SetActive(false); });
        }
    }
}