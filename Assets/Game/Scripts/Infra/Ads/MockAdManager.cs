using System;
using UnityEngine;

namespace Game.Scripts.Infra.Ads
{
    public class MockAdManager: MonoBehaviour, IAdManager
    {
        public event Action OnRvReady;
        public event Action<bool> OnRvCompleted;
        public event Action<bool> OnVideoCompleted;
        
        public bool IsRvReady()
        {
            return false;
        }

        public bool IsVideoReady()
        {
            return false;
        }

        public void ShowBanner()
        {
            
        }

        public void HideBanner()
        {
            
        }

        public bool ShowVideo(Action onCompleted = null)
        {
            onCompleted?.Invoke();
            return false;
        }

        public bool ShowRv(Action onCompleted = null)
        {
            return false;
        }
    }
}