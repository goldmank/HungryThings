using System;
using System.Collections;
using Facebook.Unity;
using Firebase.Analytics;
using Game.Scripts.Infra;
using Game.Scripts.Infra.Analytics;
using Game.Scripts.Level;
using Game.Scripts.Model;
using Game.Scripts.Ui;
using GameAnalyticsSDK;
using UnityEngine;

namespace Game.Scripts
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private LevelManager _levelManager;
        [SerializeField] private UiManager _uiManager;
       
        public UiManager Ui => _uiManager;
        public LevelManager Level => _levelManager;
        public Camera Camera => _camera;

        private static GameManager _instance;
    
        public static GameManager Get()
        {
            return _instance;
        }

        private void Awake()
        {
            _instance = this;
        }
    
        private void Start()
        {
            //FixCameraScale();
            
            InitFirebase();
            
            FB.Init(() =>
            {
                Debug.Log("FB init completed");
            });

            StartCoroutine(InitGameAnalytics());
        
            IronSource.Agent.init ("fd8b0c69");
            IronSource.Agent.validateIntegration();
        
            ModelManager.Get().GlobalPref.LastAdShown = DateTime.Now;
            Ui.GameHud.RefreshCounters();
            LoadLevel();
        
            AnalyticsService.OnAppOpen();

            if (!ModelManager.Get().GlobalPref.TutorialCompleted)
            {
                ModelManager.Get().GlobalPref.TutorialCompleted = true;
                Ui.ShowTutorial(true);
            }
            else
            {
                Ui.ShowTutorial(false);
            }
        
            ModelManager.Get().Tasker.Run(() =>
            {
                Debug.Log("GameAnalytics initialize");
                GameAnalytics.Initialize();
            }, 0.1f);
        }

        public void LoadLevel()
        {
            var dtSinceLastAd = (DateTime.Now - ModelManager.Get().GlobalPref.LastAdShown).TotalSeconds;
            if (dtSinceLastAd < Consts.Game.AdIntervalSec)
            {
                ContinueWithLoadLevel();
                return;
            }

            ModelManager.Get().GlobalPref.LastAdShown = DateTime.Now;
            ModelManager.Get().AdManager.ShowVideo(ContinueWithLoadLevel);
        }

        public void ShowGame(bool show)
        {
            _uiManager.GameHud.gameObject.SetActive(show);
            _levelManager.ShowGame(show);
        }

        private void ContinueWithLoadLevel()
        {
            _levelManager.Load(ModelManager.Get().LevelsStatus.CurrLevel);
            ShowGame(true);
        }
    
        private void FixCameraScale()
        {
            var ratio = Utils.GetScreenRatio();
            var minRatio = 0.4864f;
            var maxRatio = 0.5625f;
            var pRatio = Mathf.Clamp01((ratio - minRatio) / (maxRatio - minRatio));
            var p = 1.0f - pRatio;
            var zoomMax = 38.3f;
            var zoomMin = 34f;
            var zoom = zoomMin + p * (zoomMax - zoomMin);
            _camera.fieldOfView = zoom;
        }

        private void OnApplicationPause(bool isPaused) {                 
            IronSource.Agent.onApplicationPause(isPaused);
        }

        private void InitFirebase()
        {
            Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
                FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
                FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventLevelStart, new Parameter(FirebaseAnalytics.ParameterLevelName, "Start"));
            
                Debug.Log("dependencyStatus:" + task.Result);
                var dependencyStatus = task.Result;
                if (dependencyStatus == Firebase.DependencyStatus.Available)
                {
                    // Create and hold a reference to your FirebaseApp,
                    // where app is a Firebase.FirebaseApp property of your application class.
    
                    var app = Firebase.FirebaseApp.DefaultInstance;
                } else {
                    UnityEngine.Debug.LogError(System.String.Format(
                        "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                    // Firebase Unity SDK is not safe to use here.
                }
            });
        }

        private IEnumerator InitGameAnalytics()
        {
            yield return new WaitForSeconds(0.3f);
            GameAnalytics.Initialize();
        }
    }
}
