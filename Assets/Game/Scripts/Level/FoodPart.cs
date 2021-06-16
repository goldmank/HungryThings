using System;
using Game.Scripts.Model;
using Game.Scripts.Model.Vfx;
using UnityEngine;

namespace Game.Scripts
{
    public class FoodPart : MonoBehaviour
    {
        [SerializeField] private float _health;
        
        private float _lastDustTime;

        public event Action<FoodPart> Removed;

        public void Init(float health)
        {
            _health = health;
        }

        public void Eat(float damage)
        {
            if (_lastDustTime <= 0 || Time.time - _lastDustTime > 0.6f)
            {
                _lastDustTime = Time.time;
                ModelManager.Get().Vfx.Create(VfxType.Dust, transform.position, 2);
            }

            _health -= damage;
            if (_health > 0)
            {
                return;
            }
            Destroy(gameObject);
            Removed?.Invoke(this);
        }
    }
}
