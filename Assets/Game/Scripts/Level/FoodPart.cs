using System;
using Game.Scripts.Model;
using Game.Scripts.Model.Vfx;
using UnityEngine;

namespace Game.Scripts.Level
{
    public class FoodPart : MonoBehaviour
    {
        [SerializeField] private float _health;
        [SerializeField] private Rigidbody _body;
        [SerializeField] private FixedJoint[] _joints;

        public Rigidbody Body => _body;

        private float _lastDustTime;

        public event Action<FoodPart> Removed;
        public event Action<FoodPart> OnFloor;

#if UNITY_EDITOR
        public void SetBody(Rigidbody rigidbody)
        {
            _body = rigidbody;
        }

        public void SetJoints(FixedJoint[] joints)
        {
            _joints = joints;
        }
#endif
        
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
            Removed?.Invoke(this);
            Destroy(gameObject);
        }

        private void OnCollisionEnter(Collision other)
        {
            if (!other.collider.CompareTag("floor"))
            {
                return;
            }
            OnFloor?.Invoke(this);
        }

        public void RemoveBind(Rigidbody other)
        {
            for (var i = 0; i < _joints.Length; i++)
            {
                var joint = _joints[i];
                if (joint == null)
                {
                    continue;
                }
                if (joint.connectedBody == other)
                {
                    Destroy(joint);
                    _joints[i] = null;
                }
            }
        }
    }
}
