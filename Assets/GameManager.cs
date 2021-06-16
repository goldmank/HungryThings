using System.Collections;
using System.Collections.Generic;
using Facebook.Unity;
using GameAnalyticsSDK;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        FB.Init(() =>
        {
            Debug.Log("FB init completed");
        });
        
        StartCoroutine(InitializeGameAnalytics());
    }

    private IEnumerator InitializeGameAnalytics()
    {
        yield return new WaitForSeconds(0.5f);
        GameAnalytics.Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
