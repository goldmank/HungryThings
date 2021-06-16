using System;
using System.Collections.Generic;
using Firebase.Analytics;
using Game.Scripts.Model.World;
using GameAnalyticsSDK;
using GameAnalyticsSDK.Events;
using UnityEngine;

namespace Game.Scripts.Infra.Analytics
{
    public static class AnalyticsService
    {
        private static int _playerSelectOpenCount;
        private static DateTime? _sessionStart;
        private static int _sessionGamesPlayed;
        private static int _totalCollects;
        private static int _ranksUpCount;
        private static int _blockersCount;
        private static int _reloadCount;

        public static bool FirebaseEnabled;

        private static DateTime _startTime = DateTime.Now;
        
        public static void OnAppOpen()
        {
            var values = EventValues();
            SendEvent("app_open", values);
        }
        
        public static void OnLevelStart(int levelIndex)
        {
            var values = EventValues();
            values["level"] = levelIndex;
            SendEvent("level_start", values);
        }
        
        public static void OnLevelComplete(int levelIndex)
        {
            var values = EventValues();
            values["level"] = levelIndex;
            SendEvent("level_complete", values);
            
            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, levelIndex.ToString());
        }
        
        public static void OnStandPurchase(WorldType type)
        {
            var values = EventValues();
            values["type"] = type.ToString();
            SendEvent("buy_stand", values);
        }
        
        private static Dictionary<string, object> EventValues()
        {
            var result = new Dictionary<string, object>();
            AddUserData(result);

            // if (_sessionStart != null)
            // {
            //     var duration = (int)(MyDateTime.Now - _sessionStart.Value).TotalMinutes;
            //     result["min_in_session"] = duration;
            // }
            return result;
        }

        private static void AddUserData(Dictionary<string, object> values)
        {
            // var userData = ModelManager.Get().User;
            // values["rank"] = userData.Rank;
            // values["rank_progress"] = userData.RankProgress;
            // values["perk_1"] = userData.Perk_1;
            // values["perk_2"] = userData.Perk_2;
            // values["perk_3"] = userData.Perk_3;
            // values["bot_var"] = userData.BotVariation;
            // values["bot"] = userData.PlayerBotType;
            // values["bomb"] = userData.Bomb;
            // values["smoke"] = userData.Smoke;
            // values["stun"] = userData.Stun;
            // values["gamesPlayed"] = userData.GamesPlayed;
            // values["weaponType"] = userData.WeaponType;
            // values["skin"] = userData.BotSkinType;
            //
            // var currency = ModelManager.Get().Currency;
            // values["coins"] = currency.Coins;
        }

        private static void SendEvent(string name, Dictionary<string, object> values)
        {
            //LionStudios.Analytics.LogEvent(name, values);
            
            #if UNITY_EDITOR
                Debug.Log("Event: " + name);
                return;
            #endif
            
            try
            {
                var map = new Dictionary<string, string>();
                foreach (var v in values)
                {
                    map[v.Key] = v.Value.ToString();
                }
                //AppsFlyerSDK.AppsFlyer.sendEvent(name, map);
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
            
            UnityEngine.Analytics.Analytics.CustomEvent(name, values);
            
            // try
            // {
            //     Branch.userCompletedAction(name, values);
            // }
            // catch (Exception e)
            // {
            //     Debug.Log(e);
            // }
            
#if UNITY_WEBGL
            ModelManager.Get().WebGlWrapper.LogEvent(name, values);
#else
            try
            {
                if (FirebaseEnabled)
                {
                    var eventList = new List<Parameter>();
                    foreach (var value in values)
                    {
                        switch (value.Value)
                        {
                            case int intValue:
                                eventList.Add(new Parameter(value.Key, intValue));
                                break;
                            case string stringValue:
                                eventList.Add(new Parameter(value.Key, stringValue));
                                break;
                            case double doubleValue:
                                eventList.Add(new Parameter(value.Key, doubleValue));
                                break;
                            case Enum enumValue:
                                eventList.Add(new Parameter(value.Key, enumValue.ToString()));
                                break;
                        }
                    }
                    
                    FirebaseAnalytics.LogEvent(name, eventList.ToArray());    
                }
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
#endif
        }
    }
}