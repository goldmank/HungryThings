using System.Collections;
using System.Collections.Generic;
using Facebook.Unity;
using Firebase.Analytics;
using GameAnalyticsSDK;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        InitFirebase();
            
        FB.Init(() =>
        {
            Debug.Log("FB init completed");
        });

        StartCoroutine(InitGameAnalytics());
        
        IronSource.Agent.init ("fd8b0c69");
        IronSource.Agent.validateIntegration();

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
