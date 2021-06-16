using System;
using System.Collections.Generic;

namespace IO.Infra.Scripts.Events
{
    public class SimpleEventManager
    {
        private static SimpleEventManager _instance;

        private Dictionary<string, List<Action<EventParams>>> _actions;

        public static SimpleEventManager Get()
        {
            if (_instance == null)
            {
                _instance = new SimpleEventManager();
            }

            return _instance;
        }

        private SimpleEventManager()
        {
            _actions = new Dictionary<string, List<Action<EventParams>>>();
        }

        public void Subscribe(string eventName, Action<EventParams> triggerAction)
        {
            if (!_actions.TryGetValue(eventName, out var values))
            {
                values = new List<Action<EventParams>>();
                _actions[eventName] = values;
            }
            
            values.Add(triggerAction);
        }
        
        public void Unsubscribe(string eventName, Action<EventParams> triggerAction)
        {
            if (!_actions.TryGetValue(eventName, out var values))
            {
                return;
            }
            
            values.Remove(triggerAction);
        }

        public void TriggerEvent(string eventName, EventParams eventParams = null)
        {
            if (!_actions.TryGetValue(eventName, out var values))
            {
                return;
            }

            foreach (var action in values)
            {
                action?.Invoke(eventParams);
            }
        }
    }
}