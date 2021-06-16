using System;
using Game.Scripts.Model;
using UnityEngine;

namespace Game.Scripts.Audio
{
    public class MusicPlayer : MonoBehaviour
    {
        [SerializeField] private AudioSource _audioSource;

        private Action _onFadeComplete;
        private float _fadeProgress = 1;
        private float _maxVolume = 1;
        private float _fadeDirection;
        private float _fadeSpeed = 1;
        private float _volStart;
        private float _volEnd;
        
        public void Play(MusicData musicData, bool fadeIn = true, Action onComplete = null)
        {
            _onFadeComplete = null;
            _maxVolume = musicData.MaxVolume;
            _fadeDirection = 1;
            
            _audioSource.clip = musicData.Track;
            _audioSource.Play();
            
            if (!ModelManager.Get().GlobalPref.MusicEnabled)
            {
                _audioSource.volume = 0;
                _fadeProgress = 1;
                onComplete?.Invoke();
                return;
            }

            if (fadeIn)
            {
                _onFadeComplete = onComplete;
                _fadeProgress = 0;
                _audioSource.volume = 0;
                _volStart = 0;
                _volEnd = _maxVolume;
            }
            else
            {
                _audioSource.volume = _maxVolume;
                _fadeProgress = 1;
                onComplete?.Invoke();
            }
        }

        public void Stop(bool fadeOut = true, Action onComplete = null)
        {
            _onFadeComplete = null;
            _fadeDirection = -1;

            if (fadeOut)
            {
                _fadeProgress = 0;
                _onFadeComplete = onComplete;
                _volStart = _audioSource.volume;
                _volEnd = 0;
            }
            else
            {
                _fadeProgress = 1;
                _audioSource.volume = 0;
                onComplete?.Invoke();
            }
        }

        public void SetMute(bool mute)
        {
            _audioSource.volume = mute ? 0 : _maxVolume;
            _fadeProgress = 1;
        }
        
        public void SetFadeSpeed(float speed = 1)
        {
            _fadeSpeed = speed;
        }

        private void Update()
        {
            if (_fadeProgress >= 1)
            {
                return;
            }

            //Debug.Log("_fadeProgress: " + _fadeProgress + ", _fadeDirection:" + _fadeDirection + ", _audioSource.volume: " +_audioSource.volume);
            var step = Time.deltaTime * _fadeSpeed;
            _fadeProgress += step;
            if (_fadeProgress > 1)
            {
                _fadeProgress = 1;
            }

            var v = _volStart + _fadeProgress * (_volEnd - _volStart);
            _audioSource.volume = Mathf.Clamp(v, 0, _maxVolume);
            
            if (_fadeProgress >= 1)
            {
                _onFadeComplete?.Invoke();
                _onFadeComplete = null;
            }
        }
    }
}