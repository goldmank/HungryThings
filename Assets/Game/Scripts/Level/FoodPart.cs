using System;
using DG.Tweening;
using Game.Scripts.Model;
using Game.Scripts.Model.Vfx;
using UnityEngine;

namespace Game.Scripts.Level
{
    public class FoodPart : MonoBehaviour
    {
        [SerializeField] private float _health;
        // [SerializeField] private Rigidbody _body;
        // [SerializeField] private FixedJoint[] _joints;

        //public Rigidbody Body => _body;

        private int _coins;
        
        public event Action<FoodPart> Removed;
        public event Action<FoodPart> OnFloor;

#if UNITY_EDITOR
        public void SetBody(Rigidbody rigidbody)
        {
            //_body = rigidbody;
        }

        public void SetJoints(FixedJoint[] joints)
        {
            //_joints = joints;
        }
#endif
        
        public void Init(float health, int coins)
        {
            _health = health;
            _coins = coins;
        }

        public int Eat(float damage)
        {
            _health -= damage;
            if (_health > 0)
            {
                return 0;
            }
            Removed?.Invoke(this);
            transform.DOScale(0, 0.2f).OnComplete(() =>
            {
                Destroy(gameObject); 
            });
            return _coins;
        }

        // private void OnCollisionEnter(Collision other)
        // {
        //     if (!other.collider.CompareTag("floor"))
        //     {
        //         return;
        //     }
        //     OnFloor?.Invoke(this);
        // }
        //
        // public void RemoveBind(Rigidbody other)
        // {
        //     for (var i = 0; i < _joints.Length; i++)
        //     {
        //         var joint = _joints[i];
        //         if (joint == null)
        //         {
        //             continue;
        //         }
        //         if (joint.connectedBody == other)
        //         {
        //             Destroy(joint);
        //             _joints[i] = null;
        //         }
        //     }
        // }
    }
}
