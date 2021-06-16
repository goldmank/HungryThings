// using System;
// using System.Collections;
// using System.Collections.Generic;
// using GoogleMobileAds.Api;
// using UnityEngine;
//
// #if (UNITY_ANDROID || UNITY_IOS)
//
// namespace Game.Scripts.Infra.Ads
// {
//     public class AdManagerAdMob: MonoBehaviour, IAdManager
//     {
// #if UNITY_IOS
//         private static readonly string AppId = "ca-app-pub-9630436765811866~9415305535";
//         
//         //test
//         // private static readonly string PlacementIdBanner = "ca-app-pub-3940256099942544/6300978111";//"ca-app-pub-9630436765811866/3825412091";
//         // private static readonly string PlacementIdRv = "ca-app-pub-3940256099942544/5224354917";//"ca-app-pub-9630436765811866/7014682216";
//         // private static readonly string PlacementIdVideo = "ca-app-pub-3940256099942544/1033173712";//"ca-app-pub-9630436765811866/1023988938";
//         
//         private static readonly string PlacementIdBanner = "";
//         private static readonly string PlacementIdRv = "ca-app-pub-9630436765811866/8345766442";
//         private static readonly string PlacementIdVideo = "ca-app-pub-9630436765811866/5400038905";
// #elif UNITY_ANDROID
//         private static readonly string AppId = "ca-app-pub-9630436765811866~4086050983";
//         
//         //test
//         private static readonly string PlacementIdBanner = "ca-app-pub-9630436765811866/4662186931";
//         private static readonly string PlacementIdVideo = "ca-app-pub-9630436765811866/5207560964";
//         private static readonly string PlacementIdRv = "ca-app-pub-9630436765811866/7642152619";
// #endif
//         
//         public event Action OnRvReady;
//         public event Action<bool> OnRvCompleted;
//         public event Action<bool> OnVideoCompleted;
//
//         private Action _currRvCompleteEvent;
//         private Action _currVideoCompleteEvent;
//
//         private BannerView _bannerView;
//         private InterstitialAd _interstitial;
//         private RewardedAd _rewardedAd;
//         private float _lastRvRequest;
//
//         private bool _reRequestVideoOnNetwork;
//         private bool _reRequestRvOnNetwork;
//
//         private bool? _rvCompleted;
//         private bool? _interstitialCompleted;
//         
//         private bool _bannerActive;
//         
//         private void Start () {
//             Debug.Log("Start AdManager");
//             MobileAds.Initialize((status) =>
//             {
//                 Debug.Log("AdManager status: ");
//                 Debug.Log(status);
//             });
//
//             if (Consts.Debug)
//             {
//                 var builder = new RequestConfiguration.Builder();
//                 var testDeviceIds = new List<string> {"B34325BEB77A396EC94408D693EA6B43"};
//                 builder.SetTestDeviceIds(testDeviceIds);
//                 MobileAds.SetRequestConfiguration(builder.build());   
//             }
//
//             InitBanner();
//             _reRequestVideoOnNetwork = true;
//             _reRequestRvOnNetwork = true;
//             _lastRvRequest = -60;
//         }
//
//         private void Update()
//         {
//             if (_rvCompleted != null)
//             {
//                 HandleRvFinish(_rvCompleted.Value);
//                 _rvCompleted = null;
//             }
//
//             if (_interstitialCompleted != null)
//             {
//                 OnVideoCompleted?.Invoke(_interstitialCompleted.Value);
//                 _interstitialCompleted = null;
//             }
//
//             if (Application.internetReachability == NetworkReachability.NotReachable)
//             {
//                 return;
//             }
//
//             if (_reRequestVideoOnNetwork)
//             {
//                 Debug.Log("restore network - request video");
//                 _reRequestVideoOnNetwork = false;
//                 RequestNewInterstitial();
//             }
//
//             if (_reRequestRvOnNetwork)
//             {
//                 Debug.Log("restore network - request rv");
//                 _reRequestRvOnNetwork = false;
//                 RequestNewRv();
//             }
//         }
//
//         private void OnDestroy()
//         {
//             if (_bannerView != null)
//             {
//                 _bannerView.Destroy();
//                 _bannerView = null;
//             }
//
//             DestroyInterstitial();
//             DestroyRv();
//         }
//
//         public bool IsRvReady()
//         {
//             return null != _rewardedAd && _rewardedAd.IsLoaded();
//         }
//
//         public bool IsVideoReady()
//         {
//             return null != _interstitial && _interstitial.IsLoaded();
//         }
//
//         public void ShowBanner()
//         {
//             if (_bannerActive)
//             {
//                 Debug.Log("ShowBanner blocked - already visible");
//                 return;
//             }
//             _bannerActive = true;
//             StartCoroutine(ShowBannerWhenReady());
//         }
//         
//         public void HideBanner()
//         {
//             if (!_bannerActive)
//             {
//                 Debug.Log("HideBanner blocked - already hidden");
//                 return;
//             }
//
//             _bannerActive = false;
//             StopAllCoroutines();
//             _bannerView.Hide();
//         }
//
//         public bool ShowVideo(Action onCompleted = null)
//         {
//             if (!Consts.Ads)
//             {
//                 onCompleted?.Invoke();
//                 return false;
//             }
// #if UNITY_EDITOR
//             if (Consts.Debug)
//             {
//                 Debug.Log("Editor in debug mode - mock video");
//                 onCompleted?.Invoke();
//                 return true;
//             }
// #endif
//             if (!IsVideoReady())
//             {
//                 Debug.Log("ShowVideo fail - not ready");
//                 onCompleted?.Invoke();
//                 return false;
//             }
//             
//             _currVideoCompleteEvent = onCompleted;
//             _interstitial.Show();
//             return true;
//         }
//
//         public bool ShowRv(Action onCompleted = null)
//         {
//             if (!Consts.Ads)
//             {
//                 onCompleted?.Invoke();
//                 return false;
//             }
// #if UNITY_EDITOR
//             if (Consts.Debug)
//             {
//                 Debug.Log("Editor in debug mode - mock RV");
//                 _currRvCompleteEvent = onCompleted;
//                 StartCoroutine(DelayedMockRvFinish());
//                 return true;
//             }
// #endif
//             if (!IsRvReady())
//             {
//                 Debug.Log("ShowRv fail - not ready");
//                 RequestNewRv();
//                 return false;
//             }
//
//             _currRvCompleteEvent = onCompleted;
//             _rewardedAd.Show();
//             //SimpleEventManager.Get().TriggerEvent(Events.Events.Ads.RvStart);
//             return true;
//         }
//
//         private IEnumerator DelayedMockRvFinish()
//         {
//             yield return new WaitForSeconds(1);
//             HandleOnRvCompleted(null, null);
//         }
//
//         private void InitBanner()
//         {
//             if (!Consts.Ads)
//             {
//                 return;
//             }
//             
//             if (string.IsNullOrEmpty(PlacementIdBanner))
//             {
//                 return;
//             }
//             
//             _bannerView = new BannerView(PlacementIdBanner, AdSize.Banner, AdPosition.Bottom);
//             
//             _bannerView.OnAdLoaded += OnBannerAdLoaded;
//             _bannerView.OnAdFailedToLoad += OnBannerLoadFailed;
//             
//             ShowBanner();
//         }
//
//         private IEnumerator ShowBannerWhenReady () {
//             //while (_bannerActive)
//             {
//                 while (_bannerView == null && _bannerActive)
//                 {
//                     yield return new WaitForSeconds(0.1f);
//                 }
//
//                 if (!_bannerActive)
//                 {
//                     yield break;
//                 }
//
//                 if (_bannerView == null)
//                 {
//                     yield break;
//                 }
//                 
//                 Debug.Log("Request new banner");
//                 _bannerView.Show();
//                 var request = new AdRequest.Builder().Build();
//                 _bannerView.LoadAd(request);
//
//                 var waitTime = 20;//Economy.Game.BannerRotateDurationSec;
//                 if (waitTime < 0 || waitTime > 100)
//                 {
//                     waitTime = 20;
//                 }
//                 Debug.Log("waitTime for new banner: " + waitTime);
//                 yield return new WaitForSeconds (waitTime);
//             }
//         }
//
//         private void DestroyRv()
//         {
//             if (_rewardedAd == null)
//             {
//                 return;
//             }
//             Debug.Log("destroy old rv");
//             _rewardedAd.OnAdLoaded -= OnRvLoaded;
//             _rewardedAd.OnUserEarnedReward -= HandleOnRvCompleted;
//             _rewardedAd.OnAdFailedToShow -= OnRvShowFailed;
//             _rewardedAd.OnAdFailedToLoad -= OnRvLoadFailed;
//             _rewardedAd.OnAdClosed -= OnRvClosed;
//             _rewardedAd = null;
//         }
//
//         private void RequestNewRv()
//         {
//             if (Application.internetReachability == NetworkReachability.NotReachable)
//             {
//                 Debug.Log("no network - ignore request");
//                 _reRequestRvOnNetwork = true;
//                 return;
//             }
//
//             if (Time.time - _lastRvRequest < 30)
//             {
//                 Debug.Log("too soon from last rv request");
//                 return;
//             }
//             
//             DestroyRv();
//
//             _lastRvRequest = Time.time;
//             
//             _rewardedAd = new RewardedAd(PlacementIdRv);
//             
//             _rewardedAd.OnAdLoaded += OnRvLoaded;
//             _rewardedAd.OnUserEarnedReward += HandleOnRvCompleted;
//             _rewardedAd.OnAdFailedToShow += OnRvShowFailed;
//             _rewardedAd.OnAdFailedToLoad += OnRvLoadFailed;
//             _rewardedAd.OnAdClosed += OnRvClosed;
//             
//             Debug.Log("RequestNewRv");
//             var request = new AdRequest.Builder().Build();
//             _rewardedAd.LoadAd(request);
//         }
//
//         private void DestroyInterstitial()
//         {
//             if (_interstitial == null)
//             {
//                 return;
//             }
//             Debug.Log("destroy old video");
//             _interstitial.OnAdLoaded -= OnVideoAdLoaded;
//             _interstitial.OnAdFailedToLoad -= OnVideoAdLoadFailed;
//             _interstitial.OnAdOpening -= OnVideoAdOpen;
//             _interstitial.OnAdClosed -= OnVideoAdClosed;
//             _interstitial.Destroy();
//             _interstitial = null;
//         }
//         
//         private void RequestNewInterstitial()
//         {
//             if (string.IsNullOrEmpty(PlacementIdVideo))
//             {
//                 return;
//             }
//             DestroyInterstitial();
//             
//             Debug.Log("RequestNewVideo");
//             
//             if (Application.internetReachability == NetworkReachability.NotReachable)
//             {
//                 Debug.Log("no network - ignore request");
//                 _reRequestVideoOnNetwork = true;
//                 return;
//             }
//             
//             _interstitial = new InterstitialAd(PlacementIdVideo);
//             
//             _interstitial.OnAdLoaded += OnVideoAdLoaded;
//             _interstitial.OnAdFailedToLoad += OnVideoAdLoadFailed;
//             _interstitial.OnAdOpening += OnVideoAdOpen;
//             _interstitial.OnAdClosed += OnVideoAdClosed;
//             
//             var request = new AdRequest.Builder().Build();
//             _interstitial.LoadAd(request);
//         }
//
//         private void OnBannerLoadFailed(object sender, AdFailedToLoadEventArgs e)
//         {
//             Debug.Log("OnBannerLoadFailed: " + e.LoadAdError);
//         }
//
//         private void OnBannerAdLoaded(object sender, EventArgs e)
//         {
//             Debug.Log("OnBannerAdLoaded");
//         }
//         
//         private void OnVideoAdClosed(object sender, EventArgs e)
//         {
//             Debug.Log("OnVideoAdClosed");
//             _interstitialCompleted = true;
//             _reRequestVideoOnNetwork = true;
//             _currVideoCompleteEvent?.Invoke();
//             _currVideoCompleteEvent = null;
//         }
//
//         private void OnVideoAdOpen(object sender, EventArgs e)
//         {
//             Debug.Log("OnVideoAdOpen");
//         }
//
//         private void OnVideoAdLoadFailed(object sender, AdFailedToLoadEventArgs e)
//         {
//             Debug.Log("OnVideoLoadFailed: " + e.LoadAdError);
//             //_reRequestVideoOnNetwork = true;
//         }
//
//         private void OnVideoAdLoaded(object sender, EventArgs e)
//         {
//             Debug.Log("OnVideoAdLoaded");
//         }
//         
//         private void HandleRvFinish(bool completed)
//         {
//             Debug.Log("HandleRvFinish: " + completed);
//             if (completed) {
//                 OnRvCompleted?.Invoke(true);
//                 _currRvCompleteEvent?.Invoke();
//                 _currRvCompleteEvent = null;
//                  
//                 //SimpleEventManager.Get().TriggerEvent(Events.Events.Ads.RvCompleted);
//                 //SimpleEventManager.Get().TriggerEvent(Events.Events.Ads.RvExited);
//                 return;
//             }
//             else
//             {
//                 _currRvCompleteEvent = null;
//             }
//
//             OnRvCompleted?.Invoke(false);
//             //SimpleEventManager.Get().TriggerEvent(Events.Events.Ads.RvExited);
//         }
//         
//         private void OnRvShowFailed(object sender, AdErrorEventArgs e)
//         {
//             Debug.Log("OnRvShowFailed");
//             _rvCompleted = false;
//             //_reRequestRvOnNetwork = true;
//         }
//         
//         private void OnRvClosed(object sender, EventArgs e)
//         {
//             Debug.Log("OnRvClosed");
//             _reRequestRvOnNetwork = true;
//         }
//
//         private void OnRvLoadFailed(object sender, AdFailedToLoadEventArgs adFailedToLoadEventArgs)
//         {
//             Debug.Log("OnRvLoadFailed: " + adFailedToLoadEventArgs.LoadAdError);
//             //_reRequestRvOnNetwork = true;
//         }
//
//         private void HandleOnRvCompleted(object sender, Reward e)
//         {
//             Debug.Log("HandleOnRvCompleted");
//             _rvCompleted = true;
//         }
//
//         private void OnRvLoaded(object sender, EventArgs e)
//         {
//             Debug.Log("OnRvLoaded");
//             OnRvReady?.Invoke();
//         }
//     }
// }
//
// #endif