using System.Collections;
using System.Collections.Generic;
using GameAnalyticsSDK;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
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
