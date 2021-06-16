using System.Collections;
using System.Linq;
using Game.Scripts.Model;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Scripts.Audio
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private MusicData[] Musics;
        [SerializeField] private SoundData[] Sounds;
        [SerializeField] private MusicPlayer _pfMusicPlayer;
        [SerializeField] private AudioSource Ui;

        private float _fadeProgress = 1;
        private float _maxVolume = 1;
        private AudioCachePlayer _sfxPlayer = new AudioCachePlayer();
        private MusicPlayer _currMusicPlayer;

        private void Start()
        {
            ModelManager.Get().GlobalPref.MusicActiveChanged += OnMusicActiveChanged;
        }

        private void OnMusicActiveChanged(bool active)
        {
            if (null == _currMusicPlayer)
            {
                return;
            }
            _currMusicPlayer.SetMute(!active);
        }

        public SoundData GetSound(SoundType type)
        {
            foreach (var sound in Sounds)
            {
                if (sound.Type == type)
                {
                    return sound;
                }
            }

            return null;
        }
        
        public void PlayClick()
        {
            if (!ModelManager.Get().GlobalPref.SfxEnabled)
            {
                return;
            }
            Ui.Stop();
            var sound = GetSound(SoundType.UiTap);
            Ui.volume = sound.MaxVolume;
            Ui.clip = sound.Track;
            Ui.Play();
        }
        
        public void PlayBack()
        {
            if (!ModelManager.Get().GlobalPref.SfxEnabled)
            {
                return;
            }
            Ui.Stop();
            var sound = GetSound(SoundType.Back);
            Ui.volume = sound.MaxVolume;
            Ui.clip = sound.Track;
            Ui.Play();
        }

        public void PlayRandomSfx(SoundType[] types)
        {
            var randIndex = Random.Range(0, types.Length);
            PlaySfx(types[randIndex]);
        }

        public void PlaySfx(SoundType type, bool randPitch = false)
        {
            if (!ModelManager.Get().GlobalPref.SfxEnabled)
            {
                return;
            }

            var sound = GetSound(type);
            if (null == sound)
            {
                return;
            }

            _sfxPlayer.Play(sound, randPitch);
        }

        public void PlaySfxDelayed(SoundType soundType, float delay)
        {
            StartCoroutine(DoPlaySfxDelayed(soundType, delay));
        }

        private IEnumerator DoPlaySfxDelayed(SoundType soundType, float delay)
        {
            yield return new WaitForSeconds(delay);
            PlaySfx(soundType);
        }

        public void StopMusic()
        {
            if (_currMusicPlayer == null)
            {
                return;
            }
            _currMusicPlayer.SetFadeSpeed(0.5f);
            _currMusicPlayer.Stop(true, () =>
            {
                Destroy(_currMusicPlayer.gameObject);
                _currMusicPlayer = null;
            });
        }
        
        public void PlayMusic(MusicType type)
        {
            Debug.Log("PlayMusic: " + type);
            if (_currMusicPlayer != null)
            {
                _currMusicPlayer.Stop(true, () => { DoPlayMusic(type); });
                return;
            }

            DoPlayMusic(type);
        }

        private void DoPlayMusic(MusicType musicType)
        {
            if (null != _currMusicPlayer)
            {
                Destroy(_currMusicPlayer.gameObject);
                _currMusicPlayer = null;
            }
            
            var music = GetMusic(musicType);
            if (music == null)
            {
                return;
            }

            _currMusicPlayer = Instantiate(_pfMusicPlayer, transform);
            _currMusicPlayer.Play(music);
        }
        
        private MusicData GetMusic(MusicType type)
        {
            foreach (var music in Musics)
            {
                if (music.Type == type)
                {
                    return music;
                }
            }

            return null;
        }

        private void Update()
        {
            _sfxPlayer.Update();
        }

        public void AddMusics(MusicData[] musics)
        {
            if (null == musics)
            {
                return;
            }

            var list = Musics.ToList();
            foreach (var musicData in musics)
            {
                list.Add(musicData);
            }
            
            Musics = list.ToArray();
        }

        public void RemoveMusics(MusicData[] musics)
        {
            if (null == musics)
            {
                return;
            }
            
            var list = Musics.ToList();
            foreach (var musicData in musics)
            {
                foreach (var data in list)
                {
                    if (data.Type == musicData.Type)
                    {
                        list.Remove(data);
                        break;
                    }
                }
            }

            Musics = list.ToArray();
        }
        
        public void AddSounds(SoundData[] sounds)
        {
            if (null == sounds)
            {
                return;
            }

            var list = Sounds.ToList();
            foreach (var soundData in sounds)
            {
                list.Add(soundData);
            }
            
            Sounds = list.ToArray();
        }

        public void RemoveSounds(SoundData[] sounds)
        {
            if (null == sounds)
            {
                return;
            }
            
            var list = Sounds.ToList();
            foreach (var soundData in sounds)
            {
                foreach (var data in list)
                {
                    if (data.Type == soundData.Type)
                    {
                        list.Remove(data);
                        break;
                    }
                }
            }

            Sounds = list.ToArray();
        }
    }
}