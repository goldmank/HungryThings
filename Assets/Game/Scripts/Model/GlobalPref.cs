using System;
using Game.Scripts.Model.World;
using UnityEngine;

namespace Game.Scripts.Model
{
    public class GlobalPref : PlayerPrefObject
    {
        private static string[] IgnoreKeysOnSave = new[] {"economy", "strings", "push_token"};
        public event Action<bool> SfxActiveChanged;
        public event Action<bool> MusicActiveChanged;
        
        public GlobalPref() : base("Global")
        {
            LoadKeys();
        }
        
        protected override bool ShouldSaveKey(string key)
        {
            foreach (var s in IgnoreKeysOnSave)
            {
                if (s == key)
                {
                    return false;
                }
            }
            return true;
        }
        
        public string EconomyHash
        {
            get => GetString("economy_hash");
            set => Set("economy_hash", value);
        }
        
        public string Economy
        {
            get => GetString("economy");
            set => Set("economy", value);
        }

        public DateTime InstallDate
        {
            get => GetDateTime("install_date", DateTime.Now);
            set => Set("install_date", value);
        }
        
        public DateTime LastEconomyUpdate
        {
            get => GetDateTime("last_economy_update", DateTime.Now.AddDays(-1));
            set => Set("last_economy_update", value);
        }

        public string PushToken
        {
            get => GetString("push_token");
            set => Set("push_token", value);
        }
        
        public string Version
        {
            get => GetString("version");
            set => Set("version", value);
        }

        public bool MusicEnabled
        {
            get => GetInt("music_enabled") == 1;
            set
            {
                Set("music_enabled", value ? 1 : 0);
                MusicActiveChanged?.Invoke(value);
            }
        }

        public bool SfxEnabled
        {
            get => GetInt("sfx_enabled") == 1;
            set
            {
                Set("sfx_enabled", value ? 1 : 0);
                SfxActiveChanged?.Invoke(value);
            }
        }

        public bool VibrateEnabled
        {
            get => GetInt("vibrate_enabled") == 1;
            set => Set("vibrate_enabled", value ? 1 : 0);
        }
        
        public bool TutorialCompleted
        {
            get => GetInt("tutorial_completed") == 1;
            set => Set("tutorial_completed", value ? 1 : 0);
        }

        public DateTime RateUsLastShowTime
        {
            get => GetDateTime("rate_us_last_show_time", DateTime.Now);
            set => Set("rate_us_last_show_time", value);
        }
        
        public int RateUsShowCount
        {
            get => GetInt("rate_us_show_count", 0);
            set => Set("rate_us_show_count", value);
        }
        
        public SystemLanguage Language
        {
            get => (SystemLanguage)GetInt("language", (int)SystemLanguage.English);
            set => Set("language", (int)value);
        }
        
        public WorldType World
        {
            get => (WorldType)GetInt("stand");
            set => Set("stand", (int)value);
        }
        
        public DateTime LastAdShown
        {
            get => GetDateTime("last_ad_shown", DateTime.Now);
            set => Set("last_ad_shown", value);
        }
        
        public int ObjectsUnlockCount
        {
            get => GetInt("objects_unlock_count");
            set => Set("objects_unlock_count", value);
        }
    }
}