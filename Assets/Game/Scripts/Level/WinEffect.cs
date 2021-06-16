using System;
using System.Collections.Generic;
using System.Linq;
using Game.Scripts.Model;
using UnityEngine;

namespace Game.Scripts.Level
{
    public class WinEffect : MonoBehaviour
    {
        [SerializeField] private GameObject[] _explosions;
        [SerializeField] private GameObject _rain;

        [ContextMenu("Show")]
        public void Show()
        {
            Stop();

            var expoList = _explosions.ToList();
            expoList = expoList.OrderBy(a => Guid.NewGuid()).ToList();
            
            var delay = 0.0f;
            foreach (var explosion in expoList)
            {
                ShowExplosion(explosion, delay);
                delay += 0.2f;
            }
            
            ModelManager.Get().Tasker.Run(() =>
            {
                _rain.gameObject.SetActive(true);
            }, 1.0f);
        }

        public void Stop()
        {
            foreach (var explosion in _explosions)
            {
                explosion.gameObject.SetActive(false);
            }
            _rain.gameObject.SetActive(false);
        }

        private void ShowExplosion(GameObject expo, float delay)
        {
            ModelManager.Get().Tasker.Run(() =>
            {
                expo.gameObject.SetActive(true);
                
                ModelManager.Get().Tasker.Run(() => { expo.gameObject.SetActive(false); }, 2);
            }, delay);
        }
    }
}