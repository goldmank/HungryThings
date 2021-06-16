using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts.Audio
{
    public class AudioCachePlayer
    {
        private Queue<AudioSource> _sfxCache = new Queue<AudioSource>();
        private List<AudioSource> _sfxPlaying = new List<AudioSource>();
        private Queue<AudioSource> _sfxFinished = new Queue<AudioSource>();

        public void Play(SoundData sound, bool randPitch = false)
        {
            AudioSource audioSource;
            if (0 == _sfxCache.Count)
            {
                var go = new GameObject();
                audioSource = go.AddComponent<AudioSource>();
                audioSource.playOnAwake = false;
            }
            else
            {
                audioSource = _sfxCache.Dequeue();
            }
            audioSource.Stop();
            audioSource.clip = sound.Track;
            audioSource.volume = sound.MaxVolume;

            audioSource.pitch = 1;
            if (randPitch)
            {
                audioSource.pitch *= Random.Range(1, 1.5f);    
            }
            
            audioSource.Play();
            _sfxPlaying.Add(audioSource);
        }

        public void Update()
        {
            foreach (var audioSource in _sfxPlaying)
            {
                if (!audioSource.isPlaying)
                {
                    _sfxFinished.Enqueue(audioSource);
                }
            }

            while (_sfxFinished.Count > 0)
            {
                var audioSource = _sfxFinished.Dequeue();
                _sfxPlaying.Remove(audioSource);
                _sfxCache.Enqueue(audioSource);
            }
        }
    }
}