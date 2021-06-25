using System.Collections.Generic;
using Game.Scripts;
using Game.Scripts.Level;
using UnityEditor;
using UnityEngine;

public static class GameTools
{
    [MenuItem("GameObject/ActiveToggle _a")]
    static void ToggleActivationSelection()
    {
        var go = Selection.activeGameObject;
        go.SetActive(!go.activeSelf);
    }

    [MenuItem("Tools/Game/ClearPref")]
    public static void ClearPref()
    {
        PlayerPrefs.DeleteAll();
        Caching.ClearCache();
    }

    [MenuItem("Tools/Create/CreateSplitObject")]
    public static void CreateSplitObject()
    {
        var go = Selection.activeGameObject;
        if (!go.name.Contains("500p"))
        {
            Debug.Log("not a 500p object");
            return;
        }

        var foodParts = new List<FoodPart>();
        var meshRenderers = go.GetComponentsInChildren<MeshRenderer>();

        foreach (var meshRenderer in meshRenderers)
        {
            var obj = meshRenderer.gameObject;
            var foodPart = obj.AddComponent<FoodPart>();
            obj.AddComponent<BoxCollider>();    
            foodPart.SetBody(obj.AddComponent<Rigidbody>());
            obj.tag = "food";
            obj.layer = 9;

            foodParts.Add(foodPart);
        }
        
        BindParts(foodParts);
    }

    private static void BindParts(List<FoodPart> foodParts)
    {
        foreach (var foodPart1 in foodParts)
        {
            var boddies = new List<Rigidbody>();
            
            var body1 = foodPart1.GetComponent<Rigidbody>();
            foreach (var foodPart2 in foodParts)
            {
                if (foodPart1 == foodPart2)
                {
                    continue;
                }
            
                var body2 = foodPart2.GetComponent<Rigidbody>();
                boddies.Add(body2);
            }
            
            boddies.Sort((a, b) =>
            {
                var b1Pos = body1.transform.position;
                var da = (b1Pos - a.transform.position).magnitude;
                var db = (b1Pos - b.transform.position).magnitude;
                if (da > db)
                {
                    return 1;
                }
                if (da < db)
                {
                    return -1;
                }

                return 0;
            });

            var list = new List<FixedJoint>();
            var maxConnections = 10;
            var maxDist = 0.01f;
            for (var i = 0; i < maxConnections; i++)
            {
                if (boddies.Count <= 0)
                {
                    break;
                }

                var body2 = boddies[0];
                boddies.RemoveAt(0);

                var d = (body1.transform.position - body2.transform.position).magnitude;
                if (d > maxDist)
                {
                    continue;
                }
                
                var fixedJoint = foodPart1.gameObject.AddComponent<FixedJoint>();
                fixedJoint.connectedBody = body2;
                list.Add(fixedJoint);
            }
            
            foodPart1.SetJoints(list.ToArray());
        }
    }
}
