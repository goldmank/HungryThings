using System;
using DG.Tweening;
using Game.Scripts.Audio;
using Game.Scripts.Infra;
using Game.Scripts.Model;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.Ui
{
    public class UiManager : MonoBehaviour
    {
        [SerializeField] private GameHud _hud;
        [SerializeField] private Image _flash;
        [SerializeField] private GameObject _coins;
        [SerializeField] private GameOverScreen _gameOverScreen;
        [SerializeField] private Image _overlay;
        [SerializeField] private SettingsScreen _settingsScreen;
        [SerializeField] private LevelsScreen _levelsScreen;
        [SerializeField] private ObjectsGallery _objectsGallery;
        [SerializeField] private StoreScreen _storeScreen;
        [SerializeField] private Tutorial _tutorial;
        [SerializeField] private TimeUpPopup _timeUpPopup;
        [SerializeField] private FindAllPopup _findCompletePopup;

        public GameHud GameHud => _hud;

        private void Start()
        {
            // _gameOverScreen.gameObject.SetActive(false);
            // _settingsScreen.gameObject.SetActive(false);
            // _levelsScreen.gameObject.SetActive(false);
            // _storeScreen.gameObject.SetActive(false);
            // _objectsGallery.gameObject.SetActive(false);
            // _overlay.gameObject.SetActive(false);
            // _flash.gameObject.SetActive(false);
        }

        public void Flash(Action onComplete)
        {
            var d = 0.1f;
            
            _flash.gameObject.SetActive(true);
            Utils.SetAlpha(_flash, 0);
            _flash.DOFade(1, d).OnComplete(() =>
            {
                _flash.DOFade(0, d).OnComplete(() =>
                {
                    _flash.gameObject.SetActive(false);
                    onComplete?.Invoke();
                });
            });
        }

        public void ShowTutorial(bool show)
        {
            //_tutorial.gameObject.SetActive(show);
        }

        public void ShowGameOver()
        {
            _gameOverScreen.Show();
        }
        
        public void ShowOverlay(Action onComplete, float duration = 0.5f)
        {
            if (duration <= 0)
            {
                _overlay.gameObject.SetActive(true);
                Utils.SetAlpha(_overlay, 1);
                return;
            }
            
            _overlay.gameObject.SetActive(true);
            Utils.SetAlpha(_overlay, 0);
            _overlay.DOFade(1, duration).OnComplete(() => { onComplete?.Invoke(); });
        }

        public void HideOverlay(Action onComplete, float duration = 0.5f)
        {
            if (!_overlay.gameObject.activeSelf) // already hidden
            {
                onComplete?.Invoke();
                return;
            }
            _overlay.DOFade(0, duration).OnComplete(() => { _overlay.gameObject.SetActive(false); onComplete?.Invoke(); });
        }

        public void ShowSettings()
        {
            HideHud();
            ModelManager.Get().AudioManager.PlayClick();
            _coins.gameObject.SetActive(false);
            _settingsScreen.Show(() =>
            {
                _coins.gameObject.SetActive(true);
                ShowHud();
            });
        }

        public void ShowStore()
        {
            HideHud();
            ModelManager.Get().AudioManager.PlayClick();
            _storeScreen.Show(ShowHud);
        }

        public void ShowLevels()
        {
            HideHud();
            ModelManager.Get().AudioManager.PlayClick();
            _levelsScreen.Show(ShowHud);
        }

        public void ShowObjects()
        {
            HideHud();
            ModelManager.Get().AudioManager.PlayClick();
            _objectsGallery.Show(ShowHud);
        }

        public void ShowTimesUp()
        {
            ModelManager.Get().AudioManager.PlaySfx(SoundType.Timeout);
            _timeUpPopup.Show();
        }

        public void HideTimesUp()
        {
            _timeUpPopup.gameObject.SetActive(false);
        }
        
        public void ShowFindComplete(Sprite icon)
        {
            ModelManager.Get().AudioManager.PlaySfx(SoundType.FindAllObjects);
            _findCompletePopup.SetIcon(icon);
            _findCompletePopup.Show(0.2f, 1);
        }

        private void HideHud()
        {
            GameManager.Get().ShowGame(false);
        }
        
        private void ShowHud()
        {
            GameManager.Get().ShowGame(true);
        }
    }
}