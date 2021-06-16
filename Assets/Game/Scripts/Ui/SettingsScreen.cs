using System;
using Game.Scripts.Model;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.Ui
{
    public class SettingsScreen : MonoBehaviour
    {
        [SerializeField] private Image _sfxToggle;
        [SerializeField] private Image _vibrateToggle;
        [SerializeField] private Sprite[] _toggleStates;
        
        private Action _onClose;
        
        public void Show(Action onClose)
        {
            _onClose = onClose;
            gameObject.SetActive(true);
            _sfxToggle.sprite = _toggleStates[ModelManager.Get().GlobalPref.SfxEnabled ? 1 : 0];
            _vibrateToggle.sprite = _toggleStates[ModelManager.Get().GlobalPref.VibrateEnabled ? 1 : 0];
        }
        
        public void OnCloseClicked()
        {
            ModelManager.Get().AudioManager.PlayBack();
            _onClose?.Invoke();
            gameObject.SetActive(false);
        }
        
        public void OnSfxToggle()
        {
            ModelManager.Get().AudioManager.PlayClick();
            ModelManager.Get().GlobalPref.SfxEnabled = !ModelManager.Get().GlobalPref.SfxEnabled;
            _sfxToggle.sprite = _toggleStates[ModelManager.Get().GlobalPref.SfxEnabled ? 1 : 0];
        }

        public void OnMusicToggle()
        {
            ModelManager.Get().AudioManager.PlayClick();
            ModelManager.Get().GlobalPref.MusicEnabled = !ModelManager.Get().GlobalPref.MusicEnabled;
        }

        public void OnVibrateToggle()
        {
            ModelManager.Get().AudioManager.PlayClick();
            ModelManager.Get().GlobalPref.VibrateEnabled = !ModelManager.Get().GlobalPref.VibrateEnabled;
            _vibrateToggle.sprite = _toggleStates[ModelManager.Get().GlobalPref.VibrateEnabled ? 1 : 0];
        }
    }
}