using System;
using UnityEngine;

namespace Game.Scripts.Level
{
    public class SwipeDetector : MonoBehaviour
    {
        [SerializeField] private float _minSwipeV = 1;
        [SerializeField] private float _minSwipeSize = 400;
        
        private float _dragStartTime;
        private Vector3 _startPos;

        public enum SwipeDirection
        {
            Left,
            Right,
            Up,
            Down
        }

        public event Action<SwipeDirection> OnSwipe;
        
        private void Update()
        {
            if (Input.GetMouseButton(0))
            {
                if (_dragStartTime <= 0)
                {
                    _dragStartTime = Time.time;
                    _startPos = Input.mousePosition;
                }
            }
            else if (_dragStartTime > 0)
            {
                var d = Input.mousePosition - _startPos;
                var dt = Time.time - _dragStartTime;
                _dragStartTime = 0;
                
                Debug.Log("d: " + d.magnitude);
                if (d.magnitude < _minSwipeSize)
                {
                    Debug.Log("ignore drag - size - " + d);
                    return;
                }
                
                var v = (d / dt).magnitude;
                Debug.Log("v: " + d.magnitude);
                if (v < _minSwipeV)
                {
                    Debug.Log("ignore drag - speed - " + v);
                    return;
                }

                if (Mathf.Abs(d.x) > Mathf.Abs(d.y))
                {
                    if (d.x < 0)
                    {
                        OnSwipe?.Invoke(SwipeDirection.Left);
                    }
                    else
                    {
                        OnSwipe?.Invoke(SwipeDirection.Right);
                    }    
                }
                else
                {
                    if (d.y < 0)
                    {
                        OnSwipe?.Invoke(SwipeDirection.Down);
                    }
                    else
                    {
                        OnSwipe?.Invoke(SwipeDirection.Up);
                    }    
                }
            }
        }
    }
}