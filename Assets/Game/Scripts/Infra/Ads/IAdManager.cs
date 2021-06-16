using System;

namespace Game.Scripts.Infra.Ads
{
    public interface IAdManager
    {
        event Action OnRvReady;
        event Action<bool> OnRvCompleted;
        event Action<bool> OnVideoCompleted;
        
        bool IsRvReady();

        bool IsVideoReady();

        void ShowBanner();
        void HideBanner();

        bool ShowVideo(Action onCompleted = null);
        bool ShowRv(Action onCompleted = null);
    }
}