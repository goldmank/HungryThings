using System.Collections.Generic;
using Game.Scripts.Infra.Events;
using Game.Scripts.Model;
using IO.Infra.Scripts.Events;
using UnityEngine;

namespace Game.Scripts.Level
{
    public class EaterSpawner : MonoBehaviour
    {
        private Transform _world;
        private Eater _eater;

        private List<Eater> _eaters = new List<Eater>();

        public List<Eater> Eaters => _eaters;

        private void Start()
        {
            SimpleEventManager.Get().Subscribe(Events.Game.EaterHolderSpawn, OnEaterHolderSpawn);
            SimpleEventManager.Get().Subscribe(Events.Game.EaterHolderDiscard, OnEaterHolderDiscard);
            SimpleEventManager.Get().Subscribe(Events.Game.EaterHolderReleased, OnEaterHolderReleased);
        }
        
        private void OnDestroy()
        {
            SimpleEventManager.Get().Unsubscribe(Events.Game.EaterHolderSpawn, OnEaterHolderSpawn);
        }

        public void Init(Transform world)
        {
            _world = world;
        }

        private void Update()
        {
            if (null == _eater)
            {
                return;
            }

            UpdateEaterPosition();
        }

        private void OnEaterHolderSpawn(EventParams obj)
        {
            var type = (EaterType)obj.GetInt(Events.Game.ParamEaterType);
            var prefab = ModelManager.Get().Eaters.GetEater(type);
            if (prefab == null || prefab.Prefab == null)
            {
                return;
            }
            _eater = Instantiate(prefab.Prefab, _world);
            
            UpdateEaterPosition();
        }
        
        private void OnEaterHolderDiscard(EventParams obj)
        {
            if (null == _eater)
            {
                return;
            }
            
            Destroy(_eater.gameObject);
            _eater = null;
        }
        
        private void OnEaterHolderReleased(EventParams obj)
        {
            if (null == _eater)
            {
                return;
            }
            
            _eaters.Add(_eater);
            
            _eater.Init(1);
            _eater = null;
        }

        private void UpdateEaterPosition()
        {
            var mousePos = Input.mousePosition;
            mousePos.y -= 550;
            
            var ray = GameManager.Get().Camera.ScreenPointToRay(mousePos);
            if (!Physics.Raycast(ray, out var hit))
            {
                return;
            }

            var pos = hit.point; 
            pos.y = 2;
            
            //Debug.Log(pos);
            _eater.transform.position = pos;
        }
    }
}