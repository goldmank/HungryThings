using System.Collections.Generic;
using UnityEngine;

namespace IO.Infra.Scripts.Events
{
    public class EventParams
    {
        //TODO: add cache of EventParams
        
        private readonly Dictionary<string, object> _params;

        public EventParams(Dictionary<string, object> data = null)
        {
            if (data == null)
            {
                data = new Dictionary<string, object>();
            }

            _params = data;
        }

        public string GetString(string key)
        {
            if (_params.TryGetValue(key, out var value))
            {
                return (string) value;
            }

            return null;
        }
        
        public int GetInt(string key)
        {
            if (_params.TryGetValue(key, out var value))
            {
                return (int) value;
            }

            return 0;
        }
        
        public float GetFloat(string key)
        {
            if (_params.TryGetValue(key, out var value))
            {
                return (float) value;
            }

            return 0;
        }
        
        public bool GetBool(string key)
        {
            if (_params.TryGetValue(key, out var value))
            {
                return (bool) value;
            }

            return false;
        }
        
        public Vector3 GetVector3(string key)
        {
            if (_params.TryGetValue(key, out var value))
            {
                return (Vector3) value;
            }

            return Vector3.zero;
        }
        
        public GameObject GetGameObject(string key)
        {
            if (_params.TryGetValue(key, out var value))
            {
                return (GameObject) value;
            }

            return null;
        }
        
        public Camera GetCamera(string key)
        {
            if (_params.TryGetValue(key, out var value))
            {
                return (Camera) value;
            }

            return null;
        }

        public object GetObject(string key)
        {
            if (_params.TryGetValue(key, out var value))
            {
                return value;
            }

            return null;
        }

        public class Builder
        {
            private Dictionary<string, object> _params;

            public Builder()
            {
                _params = new Dictionary<string, object>();
            }

            public Builder AddObject(string key, object value)
            {
                _params[key] = value;
                return this;
            }
            
            public Builder Add(string key, string value)
            {
                _params[key] = value;
                return this;
            }
            
            public Builder Add(string key, int value)
            {
                _params[key] = value;
                return this;
            }
            
            public Builder Add(string key, float value)
            {
                _params[key] = value;
                return this;
            }
            
            public Builder Add(string key, bool value)
            {
                _params[key] = value;
                return this;
            }
            
            public Builder Add(string key, Vector3 value)
            {
                _params[key] = value;
                return this;
            }
            
            public Builder Add(string key, GameObject value)
            {
                _params[key] = value;
                return this;
            }
            
            public Builder Add(string key, Camera value)
            {
                _params[key] = value;
                return this;
            }

            public EventParams build()
            {
                return new EventParams(_params);
            }
        }
    }
}