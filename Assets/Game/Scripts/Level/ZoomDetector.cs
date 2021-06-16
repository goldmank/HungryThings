using System;
using UnityEngine;

namespace Game.Scripts.Level
{
    public class ZoomDetector : MonoBehaviour
    {
        [SerializeField] private float _sensitivity = 1;

        public event Action<float> OnZoom;
        
        private void Update()
        {
            var wheel = Input.mouseScrollDelta.y * _sensitivity;
            OnZoom?.Invoke(wheel);
        }
    }
}