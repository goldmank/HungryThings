using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts.Infra
{
    public class Tasker : MonoBehaviour
    {
        private class DelayedAction
        {
            public Action Action;
            public float RunTime;
        }
        
        private LinkedList<DelayedAction> _actions = new LinkedList<DelayedAction>();
        
        private void Update()
        {
            var now = Time.time;
            for(var iter = _actions.First; iter != null; )
            {
                var next = iter.Next;
                if (now >= iter.Value.RunTime)
                {
                    iter.Value.Action?.Invoke();
                    _actions.Remove(iter); // as a side effect it.Next == null
                }
                
                iter = next;
            }
        }
        
        public void Run(Action action, float delay)
        {
            var delayedAction = new DelayedAction {Action = action, RunTime = Time.time + delay};
            _actions.AddLast(delayedAction);
        }
    }
}