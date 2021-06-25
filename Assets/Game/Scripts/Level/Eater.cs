using System;
using UnityEngine;

namespace Game.Scripts.Level
{
    public class Eater : MonoBehaviour
    {
        [SerializeField] private float _power;
        [SerializeField] private Collider _collider;

        private Rigidbody _body;
        private Vector3? _target;
        private float _lastTargetSet;
        private float _lastEat;

        private void Start()
        {
            if (null != _collider)
            {
                _collider.enabled = false;   
            }
        }
        
        public void Init(float power)
        {
            _power = power;
            _body = gameObject.AddComponent<Rigidbody>();
            _body.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY |
                                RigidbodyConstraints.FreezeRotationZ; 
            if (null == _collider)
            {
                _collider = gameObject.AddComponent<SphereCollider>();    
            }
            _collider.enabled = true;
        }
    
        void Update()
        {
            if (_body == null)
            {
                return;
            }
            if (Math.Abs(_body.velocity.y) > 0.01f)
            {
                return;
            }

            if (Time.time - _lastTargetSet > 1)
            {
                Debug.Log("reset target after 1 sec");
                _target = null;
            }

            if (_target != null)
            {
                var d = _target.Value - transform.position;
                Debug.Log("d="+d.magnitude);
                if (d.magnitude < 0.05f)
                {
                    var currFood = GameManager.Get().Level.GetFood();
                    if (null != currFood)
                    {
                        var currPart = currFood.GetClosetPart(transform);
                        if (currPart != null)
                        {
                            var foodPart = currPart.GetComponent<FoodPart>();
                            foodPart.Eat(1);   
                        }
                    }
                    _target = null;
                    _body.velocity = Vector3.zero;
                }
                else
                {
                    var a = d.normalized;
                    var v = 100;
                    _body.velocity = new Vector3(a.x * v * Time.deltaTime, 0, a.z * v * Time.deltaTime);
                }
                return;
            }
        
            var food = GameManager.Get().Level.GetFood();
            if (null == food)
            {
                return;
            }

            var part = food.GetClosetPart(transform);
            if (null == part)
            {
                return;
            }
            
            Debug.Log("target:" + part.name);

            var target = part.transform.position;
            target.y = transform.position.y;
            _target = target;
            _lastTargetSet = Time.time;
        }

        public void SetFood(FoodObject foodObject)
        {
        
        }

        private void OnCollisionStay(Collision other)
        {
            if (other.collider.CompareTag("food"))
            {
                Debug.Log("ant collide food");
                var foodPart = other.collider.GetComponent<FoodPart>();
                foodPart.Eat(_power * Time.deltaTime);
                return;
            }
        
            // if (other.collider.CompareTag("body"))
            // {
            //     Debug.Log("ant collide body");
            //     var bodyPart = other.collider.GetComponent<BodyPart>();
            //     if (bodyPart.HasBindedParts())
            //     {
            //         var foodPart = bodyPart.GetClosetPart(transform.position);
            //         foodPart.Eat(_power * Time.deltaTime);
            //     }
            //     else
            //     {
            //         bodyPart.RemoveAll();
            //     }
            // }
        }
    }
}
