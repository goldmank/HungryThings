using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Game.Scripts.Model
{
    public class PlayerPrefObject
    {
        private string _name;
        private JObject _keys;

        public PlayerPrefObject(string name = "")
        {
            _name = name;
        }

        public JObject ToJson()
        {
            try
            {
                var json = new JObject();

                foreach (var kv in _keys)
                {
                    var type = (string)kv.Value;
                    var keyName = kv.Key;
                    if (!ShouldSaveKey(keyName))
                    {
                        continue;
                    }
                    if (type == "s")
                    {
                        json[keyName] = GetString(keyName);
                    }
                    else if (type == "i")
                    {
                        json[keyName] = GetInt(keyName);
                    }
                    else if (type == "f")
                    {
                        json[keyName] = GetFloat(keyName);
                    }
                    else if (type == "a")
                    {
                        try
                        {
                            json[keyName] = GetArray(keyName);
                        }
                        catch (Exception e)
                        {
                            Debug.Log(e);
                        }
                    }
                }

                return json;
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }

            return new JObject();
        }

        protected virtual bool ShouldSaveKey(string key)
        {
            return true;
        }

        public void SetFromJson(JObject json)
        {
            try
            {
                foreach (var kv in json)
                {
                    var keyName = kv.Key;
                    var value = kv.Value;
                    switch (kv.Value.Type)
                    {
                        case JTokenType.Array:
                        {
                            Set(keyName, (JArray)value);
                        }
                            break;
                        case JTokenType.String:
                        {
                            Set(keyName, (string)value);
                        }
                            break;
                        case JTokenType.Integer:
                        {
                            Set(keyName, (int)value);
                        }
                            break;
                        case JTokenType.Float:
                        {
                            Set(keyName, (float)value);
                        }
                            break;
                    }
                }
                PlayerPrefs.Save();
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }

        public bool Has(string key)
        {
            return PlayerPrefs.HasKey(Key(key));
        }
        
        public string GetString(string key)
        {
            return PlayerPrefs.GetString(Key(key));
        }

        public int GetInt(string key, int defaultValue = 0)
        {
            return PlayerPrefs.GetInt(Key(key), defaultValue);
        }

        public float GetFloat(string key, float defaultValue = 0.0f)
        {
            return PlayerPrefs.GetFloat(Key(key), defaultValue);
        }
        
        public DateTime GetDateTime(string key, DateTime defaultValue, bool setDefault = false)
        {
            var value = PlayerPrefs.GetString(Key(key), "");
            if (string.IsNullOrEmpty(value))
            {
                if (setDefault)
                {
                    Set(key, defaultValue);
                }
                return defaultValue;
            }

            var bin = Convert.ToInt64(value);
            return DateTime.FromBinary(bin);
        }
        
        public JArray GetArray(string key)
        {
            var str = PlayerPrefs.GetString(Key(key));
            if (!string.IsNullOrEmpty(str))
            {
                try
                {
                    return JArray.Parse(str);
                }
                catch (Exception e)
                {
                    Debug.Log(e);
                }    
            }
            return new JArray();
        }
        
        public void Set(string key, string value)
        {
            PlayerPrefs.SetString(Key(key), value);
            PlayerPrefs.Save();
            AddKey(key, "s");
        }
        
        public void Set(string key, int value)
        {
            PlayerPrefs.SetInt(Key(key), value);
            PlayerPrefs.Save();
            AddKey(key, "i");
        }
        
        public void Set(string key, float value)
        {
            PlayerPrefs.SetFloat(Key(key), value);
            PlayerPrefs.Save();
            AddKey(key, "f");
        }
        
        public void Set(string key, DateTime value)
        {
            PlayerPrefs.SetString(Key(key), value.ToBinary().ToString());
            PlayerPrefs.Save();
            AddKey(key, "s");
        }
        
        public void Set(string key, JArray value)
        {
            PlayerPrefs.SetString(Key(key), value.ToString(Formatting.None));
            PlayerPrefs.Save();
            AddKey(key, "a");
        }

        private string Key(string key)
        {
            return _name + key;
        }
        
        public void LoadKeys()
        {
            _keys = new JObject();

            try
            {
                var keysCache = PlayerPrefs.GetString(Key("_keys_"));
                if (!string.IsNullOrEmpty(keysCache))
                {
                    _keys = JObject.Parse(keysCache);    
                }
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }

        private void SaveKeys()
        {
            try
            {
                PlayerPrefs.SetString(Key("_keys_"), _keys.ToString());
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }

        private void AddKey(string key, string type)
        {
            if (_keys.TryGetValue(key, out var token))
            {
                return;
            }
            
            Debug.Log("AddKey: " + key + ", type=" + type);

            _keys[key] = type;
            SaveKeys();
        }
    }
}