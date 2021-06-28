using System;
using Game.Scripts.Model;
using Game.Scripts.Model.Vfx;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Scripts.Level
{
    public class Eater : MonoBehaviour
    {
        [SerializeField] private float _power;
        [SerializeField] private Collider _collider;
        [SerializeField] private Transform _obj;

        private Rigidbody _body;
        private Vector3? _target;
        private float _lastTargetSet;
        private float _lastEat;
        private float _spawnTime;
        private float _lastDustTime;

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
            _body.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ; 
            if (null == _collider)
            {
                _collider = gameObject.AddComponent<SphereCollider>();    
            }
            _collider.enabled = true;
            _spawnTime = Time.time;
        }
    
        void Update()
        {
            if (Time.time - _spawnTime < 0.5f)
            {
                return;
            }
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
                //Debug.Log("reset target after 1 sec");
                //_target = null;
            }

            if (_target != null)
            {
                var dist = _target.Value - transform.position;
                var d = Mathf.Sqrt(dist.x * dist.x + dist.z * dist.z);
                //Debug.Log("d="+d.magnitude);
                if (d < 0.025f)
                {
                    //Debug.Log("reach target");
                    // var currFood = GameManager.Get().Level.GetFood();
                    // if (null != currFood)
                    // {
                    //     var currPart = currFood.GetClosetFoodPart(transform);
                    //     if (currPart != null)
                    //     {
                    //         d = currPart.position - transform.position;
                    //         if (d.magnitude < 0.025f)
                    //         {
                    //             var foodPart = currPart.GetComponent<FoodPart>();
                    //             foodPart.Eat(1);
                    //         }
                    //     }
                    // }
                    _target = null;
                    _body.velocity = Vector3.zero;
                }
                else
                {
                    //var a = Mathf.Atan2(dist.y,dist.x);
                    //Debug.Log("a="+a);
                    var a = Mathf.Atan2(dist.z,dist.x);
                    //Debug.Log(a);
                    var v = 150;
                    _body.velocity = new Vector3(Mathf.Cos(a) * v * Time.deltaTime, 0, Mathf.Sin(a) * v * Time.deltaTime);
                    _obj.localRotation = Quaternion.Euler(0, 0, -144 + a * Mathf.Rad2Deg);
                }

                var p = transform.position;
                //Debug.Log("(" + p.x + "," + p.z + ") -> (" + _target.Value.x + "," + _target.Value.y + "," + _target.Value.z + ")   d=" + d);
                return;
            }
        
            var food = GameManager.Get().Level.GetFood();
            if (null == food)
            {
                return;
            }

            var part = food.GetClosetFoodPart(transform, true);
            if (null == part)
            {
                return;
            }
            
            var target = part.transform.position;
            target.y = transform.position.y;
            _target = target;// + new Vector3(Random.Range(-0.5f, 0.5f), 0, Random.Range(-0.5f, 0.5f));
            _lastTargetSet = Time.time;
            
            Debug.Log("_from_:" + transform.position.x+","+transform.position.y+","+transform.position.z);
            Debug.Log("_to_:" + part.name + " - " + target.x+","+target.y+","+target.z);
        }

        public void SetFood(FoodObject foodObject)
        {
        
        }

        private void OnCollisionStay(Collision other)
        {
            if (other.collider.CompareTag("food"))
            {
                //Debug.Log("ant collide food");
                var foodPart = other.collider.GetComponent<FoodPart>();
                foodPart.Eat(_power * Time.deltaTime);
                
                if (_lastDustTime <= 0 || Time.time - _lastDustTime > 0.6f)
                {
                    _lastDustTime = Time.time;
                    ModelManager.Get().Vfx.Create(VfxType.Dust, transform.position, 2);
                }
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
