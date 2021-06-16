using System.Collections.Generic;
using Game.Scripts.Ui;
using UnityEngine;

namespace Game.Scripts.Model.Vfx
{
    [CreateAssetMenu(fileName = "VfxCatalog", menuName = "Create Vfx Catalog", order = 0)]
    public class VfxCatalog : ScriptableObject
    {
        public VfxData[] Vfxs;
        public CoinFloatReward CoinFloatReward;
        
        private Dictionary<VfxType, List<GameObject>> _cache = new Dictionary<VfxType, List<GameObject>>();
        
        public void Create(VfxType type, Vector3 position, float liveDuration)
        {
            if (_cache.TryGetValue(type, out var cacheList) && cacheList.Count > 0)
            {
                var instance = cacheList[0];
                cacheList.RemoveAt(0);
                instance.transform.position = position;
                instance.gameObject.SetActive(true);
                ModelManager.Get().Tasker.Run(() =>
                {
                    instance.gameObject.SetActive(false);
                    cacheList.Add(instance);
                }, liveDuration);
                
                return;
            }
            
            var prefab = GetVfxPrefab(type);
            if (null == prefab)
            {
                return;
            }
            
            var newInstance = Instantiate(prefab, position, Quaternion.identity);
            if (null == cacheList)
            {
                cacheList = new List<GameObject>();
                _cache[type] = cacheList;
            }
            ModelManager.Get().Tasker.Run(() =>
            {
                newInstance.gameObject.SetActive(false);
                cacheList.Add(newInstance);
            }, liveDuration);
        }
        
        private GameObject GetVfxPrefab(VfxType type)
        {
            foreach (var vfxData in Vfxs)
            {
                if (vfxData.Type == type)
                {
                    return vfxData.Prefab;
                }
            }

            return null;
        }
    }
}