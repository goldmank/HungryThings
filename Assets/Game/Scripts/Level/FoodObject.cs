using System;
using System.Collections.Generic;
using Game.Scripts.Level;
using Game.Scripts.Model;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Scripts
{
    public class FoodObject : MonoBehaviour
    {
        //[SerializeField] private Transform _body;
        //[SerializeField] private BodyPart _pfBodyPart;
        [SerializeField] private Rigidbody _mainBody;
        [SerializeField] private Collider _mainCollider;
        [SerializeField] private GameObject _parts;
        [SerializeField] private GameObject _simple;
    
        //private List<BodyPart> _bodyParts;
        private List<FoodPart> _foodParts;
        private HashSet<FoodPart> _foodOnFloor;

        public event Action<FoodObject> FoodReady;

        private void Start()
        {
            FetchFoodParts();
        
            // Destroy(_mainBody);
            // _mainBody = null;
            // Destroy(_mainCollider);
            // _mainCollider = null;
            //
            // BuildBodyParts();
        }
    
        private void OnStandStill()
        {
            Destroy(_mainBody);
            _mainBody = null;
            Destroy(_mainCollider);
            _mainCollider = null;

            _parts.gameObject.SetActive(true);
            _simple.gameObject.SetActive(false);

            FoodReady?.Invoke(this);
        }

        private void Update()
        {
            if (_mainBody == null)
            {
                return;
            }
        
            var v = _mainBody.velocity.magnitude;
            Debug.Log(v);
            if (v < 0.0001f)
            {
                //BuildBodyParts();

                OnStandStill();

                // foreach (var foodPart in _foodParts)
                // {
                //     var boxCollider = foodPart.GetComponent<BoxCollider>();
                //     Destroy(boxCollider);
                //
                //     var meshCollider = foodPart.gameObject.AddComponent<BoxCollider>();
                //     foodPart.gameObject.AddComponent<Rigidbody>();
                //     
                //     foodPart.Removed += FoodPartOnRemoved;
                // }
                //
                // foreach (var foodPart1 in _foodParts)
                // {
                //     var body1 = foodPart1.GetComponent<Rigidbody>();
                //     foreach (var foodPart2 in _foodParts)
                //     {
                //         if (foodPart1 == foodPart2)
                //         {
                //             continue;
                //         }
                //     
                //         var body2 = foodPart2.GetComponent<Rigidbody>();
                //
                //         var d = (body1.transform.position - body2.transform.position).magnitude;
                //         if (d > 0.3f)
                //         {
                //             continue;
                //         }
                //         
                //         var fixedJoint = foodPart1.gameObject.AddComponent<FixedJoint>();
                //         fixedJoint.connectedBody = body2;
                //     }    
                // }
            
            
            }
        }

        private void FoodPartOnRemoved(FoodPart obj)
        {
            foreach (var foodPart in _foodParts)
            {
                foodPart.RemoveBind(obj.Body);
            }
            _foodParts.Remove(obj);
            _foodOnFloor.Remove(obj);
        }
        
        private void FoodOnFloor(FoodPart obj)
        {
            _foodOnFloor.Add(obj);
        }

        private void FetchFoodParts()
        {
            _foodParts = new List<FoodPart>();
            _foodOnFloor = new HashSet<FoodPart>();

            var parts = _parts.transform.GetComponentsInChildren<FoodPart>();
            foreach (var foodPart in parts)
            {
                foodPart.Init(0);//Random.Range(0,10));
                foodPart.Removed += FoodPartOnRemoved;
                foodPart.OnFloor += FoodOnFloor;
                _foodParts.Add(foodPart);
            }
        }

        // private void BuildBodyParts()
        // {
        //     var min = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
        //     var max = new Vector3(float.MinValue, float.MinValue, float.MinValue);
        //     foreach (var foodPart in _foodParts)
        //     {
        //         var pos = foodPart.transform.position;
        //         min.x = Mathf.Min(min.x, pos.x);
        //         min.y = Mathf.Min(min.y, pos.y);
        //         min.z = Mathf.Min(min.z, pos.z);
        //         max.x = Mathf.Max(max.x, pos.x);
        //         max.y = Mathf.Max(max.y, pos.y);
        //         max.z = Mathf.Max(max.z, pos.z);
        //     }
        //
        //     min = _mainCollider.bounds.min + new Vector3(0.08f, 0.08f, 0.08f);
        //     max = _mainCollider.bounds.max;
        //
        //     var size = max - min;
        //     Debug.Log(size);
        //
        //     var count = 9;
        //     var bodyPartSize = size / count;
        //     Debug.Log(bodyPartSize);
        //
        //     var matrix = new BodyPart[count, count, count];
        //     
        //     _bodyParts = new List<BodyPart>();
        //     
        //     var xIter = 0;
        //     for (var x = 0; x < count; x++)
        //     {
        //         var yIter = 0;
        //         for (var y = 0; y < count; y++)
        //         {
        //             var zIter = 0;
        //             for (var z = 0; z < count; z++)
        //             {
        //                 var bodyPart = Instantiate(_pfBodyPart, _body);
        //                 bodyPart.transform.localScale = bodyPartSize/3;
        //                 var bodyPos = new Vector3
        //                 {
        //                     x = min.x + x * bodyPartSize.x,
        //                     y = min.y + y * bodyPartSize.y,
        //                     z = min.z + z * bodyPartSize.z
        //                 };
        //                 bodyPart.name = x + "-" + y + "-" + z;
        //                 bodyPart.transform.position = bodyPos;
        //                 
        //                 bodyPart.Removed += BodyPartOnRemoved;
        //                 _bodyParts.Add(bodyPart);
        //                 
        //                 foreach (var foodPart in _foodParts)
        //                 {
        //                     if (foodPart.HasBody())
        //                     {
        //                         continue;
        //                     }
        //                     
        //                     var pos = foodPart.transform.position;
        //                     var d = pos - bodyPos;
        //                     if (d.x < bodyPartSize.x / 2 && d.y < bodyPartSize.y / 2 && d.z < bodyPartSize.z)
        //                     {
        //                         //foodPart.gameObject.SetActive(false);
        //                         bodyPart.BindFoodPart(foodPart);
        //                         foodPart.SetBody(bodyPart);
        //
        //                         foodPart.transform.parent = bodyPart.transform;
        //                     }
        //                 }
        //                 
        //                 matrix[xIter, yIter, zIter] = bodyPart;
        //                 zIter++;
        //             }
        //
        //             yIter++;
        //         }
        //
        //         xIter++;
        //     }
        //
        //     
        //     // remove bottom
        //     // for (var y = 0; y < 2; y++)
        //     // {
        //     //     for (var x = 0; x < xCount + pad * 2; x++)
        //     //     {
        //     //         for (var z = 0; z < zCount + pad * 2; z++)
        //     //         {
        //     //             var bodyPart = matrix[x, y, z];
        //     //             if (bodyPart == null)
        //     //             {
        //     //                 continue;
        //     //             }
        //     //             if (!bodyPart.HasBindedParts())
        //     //             {
        //     //                 Destroy(bodyPart.gameObject);
        //     //                 matrix[x, y, z] = null;
        //     //             }
        //     //         }
        //     //     }
        //     // }
        //     //
        //     // // remove top
        //     // for (var y = 0; y < 2; y++)
        //     // {
        //     //     var ty = yCount + pad * 2 - 1 - y;
        //     //     for (var x = 0; x < xCount + pad * 2; x++)
        //     //     {
        //     //         for (var z = 0; z < zCount + pad * 2; z++)
        //     //         {
        //     //             var bodyPart = matrix[x, ty, z];
        //     //             if (bodyPart == null)
        //     //             {
        //     //                 continue;
        //     //             }
        //     //             if (!bodyPart.HasBindedParts())
        //     //             {
        //     //                 Destroy(bodyPart.gameObject);
        //     //                 matrix[x, ty, z] = null;
        //     //             }
        //     //         }
        //     //     }
        //     // }
        //     //
        //     // // remove side
        //     // for (var z = 0; z < 2; z++)
        //     // {
        //     //     var tz = zCount + pad * 2 - 1 - z;
        //     //     for (var x = 0; x < xCount + pad * 2; x++)
        //     //     {
        //     //         for (var y = 0; y < yCount + pad * 2; y++)
        //     //         {
        //     //             var bodyPart = matrix[x, y, tz];
        //     //             if (bodyPart == null)
        //     //             {
        //     //                 continue;
        //     //             }
        //     //             if (!bodyPart.HasBindedParts())
        //     //             {
        //     //                 Destroy(bodyPart.gameObject);
        //     //                 matrix[x, y, tz] = null;
        //     //             }
        //     //         }
        //     //     }
        //     // }
        //     //
        //     // // remove side
        //     // for (var z = 0; z < 2; z++)
        //     // {
        //     //     for (var x = 0; x < xCount + pad * 2; x++)
        //     //     {
        //     //         for (var y = 0; y < yCount + pad * 2; y++)
        //     //         {
        //     //             var bodyPart = matrix[x, y, z];
        //     //             if (bodyPart == null)
        //     //             {
        //     //                 continue;
        //     //             }
        //     //             if (!bodyPart.HasBindedParts())
        //     //             {
        //     //                 Destroy(bodyPart.gameObject);
        //     //                 matrix[x, y, z] = null;
        //     //             }
        //     //         }
        //     //     }
        //     // }
        //     //
        //     // // remove side
        //     // for (var x = 0; x < 2; x++)
        //     // {
        //     //     for (var z = 0; z < zCount + pad * 2; z++)
        //     //     {
        //     //         for (var y = 0; y < yCount + pad * 2; y++)
        //     //         {
        //     //             var bodyPart = matrix[x, y, z];
        //     //             if (bodyPart == null)
        //     //             {
        //     //                 continue;
        //     //             }
        //     //             if (!bodyPart.HasBindedParts())
        //     //             {
        //     //                 Destroy(bodyPart.gameObject);
        //     //                 matrix[x, y, z] = null;
        //     //             }
        //     //         }
        //     //     }
        //     // }
        //     //
        //     // // remove side
        //     // for (var x = 0; x < 2; x++)
        //     // {
        //     //     var tx = xCount + pad * 2 - 1 - x;
        //     //     for (var z = 0; z < zCount + pad * 2; z++)
        //     //     {
        //     //         for (var y = 0; y < yCount + pad * 2; y++)
        //     //         {
        //     //             var bodyPart = matrix[tx, y, z];
        //     //             if (bodyPart == null)
        //     //             {
        //     //                 continue;
        //     //             }
        //     //             if (!bodyPart.HasBindedParts())
        //     //             {
        //     //                 Destroy(bodyPart.gameObject);
        //     //                 matrix[tx, y, z] = null;
        //     //             }
        //     //         }
        //     //     }
        //     // }
        //
        //     for (var x = 0; x < count; x++)
        //     {
        //         for (var y = 0; y < count; y++)
        //         {
        //             for (var z = 0; z < count; z++)
        //             {
        //                 var bodyPart = matrix[x, y, z];
        //                 if (bodyPart == null)
        //                 {
        //                     continue;
        //                 }
        //
        //                 if (x + 1 < count)
        //                 {
        //                     bodyPart.JointTo(matrix[x + 1, y, z]);
        //                 }
        //                 if (x - 1 >= 0)
        //                 {
        //                     bodyPart.JointTo(matrix[x - 1, y, z]);
        //                 }
        //                 if (y + 1 < count)
        //                 {
        //                     bodyPart.JointTo(matrix[x, y + 1, z]);
        //                 }
        //                 if (y - 1 >= 0)
        //                 {
        //                     bodyPart.JointTo(matrix[x, y - 1, z]);
        //                 }
        //                 if (z + 1 < count)
        //                 {
        //                     bodyPart.JointTo(matrix[x, y, z + 1]);
        //                 }
        //                 if (z - 1 >= 0)
        //                 {
        //                     bodyPart.JointTo(matrix[x, y, z - 1]);
        //                 }
        //             }
        //         }   
        //     }
        //     
        //     // for (var x = 1; x < xCount + pad * 2; x++)
        //     // {
        //     //     for (var y = 1; y < yCount + pad * 2; y++)
        //     //     {
        //     //         for (var z = 1; z < zCount + pad * 2; z++)
        //     //         {
        //     //             var bodyPart = matrix[x, y, z];
        //     //             if (bodyPart == null)
        //     //             {
        //     //                 continue;
        //     //             }
        //     //             
        //     //             bodyPart.JointTo(matrix[x, y, z - 1]);
        //     //             bodyPart.JointTo(matrix[x, y - 1, z]);
        //     //             bodyPart.JointTo(matrix[x - 1, y, z]);
        //     //             bodyPart.JointTo(matrix[x - 1, y - 1, z]);
        //     //             bodyPart.JointTo(matrix[x - 1, y - 1, z - 1]);
        //     //             bodyPart.JointTo(matrix[x, y - 1, z - 1]);
        //     //             bodyPart.JointTo(matrix[x - 1, y, z - 1]);
        //     //         }
        //     //     }   
        //     // }
        // }

        // private void BodyPartOnRemoved(BodyPart obj)
        // {
        //     _bodyParts.Remove(obj);
        // }

        public Transform GetClosetPart(Transform eater)
        {
            return GetClosetFoodPart(eater);

            // var body = GetClosetBodyPart(pos);
            // if (null == body)
            // {
            //     return null;
            // }
            //
            // return body.transform;
        }
    
        // private BodyPart GetClosetBodyPart(Vector3 pos)
        // {
        //     if (_bodyParts == null)
        //     {
        //         return null;
        //     }
        //     
        //     var bestD = float.MaxValue;
        //     BodyPart bestPart = null;
        //     foreach (var bodyPart in _bodyParts)
        //     {
        //         var dVec = bodyPart.transform.position - pos;
        //         if (dVec.y > 0.1f)
        //         {
        //             continue;
        //         }
        //         var d = Mathf.Sqrt(dVec.x * dVec.x + dVec.z * dVec.z);
        //         if (d < bestD)
        //         {
        //             bestD = d;
        //             bestPart = bodyPart;
        //         }
        //     }
        //
        //     return bestPart;
        // }
    
        private Transform GetClosetFoodPart(Transform eater)
        {
            var pos = eater.position;
            
            var bestD = float.MaxValue;
            Transform bestPart = null;
            foreach (var bodyPart in _foodOnFloor)
            {
                var partPos = bodyPart.transform.position;
                var dVec = partPos - pos;
                var d = Mathf.Sqrt(dVec.x * dVec.x + dVec.z * dVec.z);
                if (d < bestD)
                {
                    bestD = d;
                    bestPart = bodyPart.transform;
                }
                //Debug.Log(bodyPart.name + " - " + partPos.x + "," + partPos.y + "," + partPos.z + " = " + d);
            }

            // var p = bestPart.position;
            // Debug.Log("GetClosetFoodPart(" + pos.x + ", " + pos.y + ", " + pos.z + ") = " + bestD + " - (" + p.x + "," + p.y + "," + p.z + ")");

            return bestPart;
        }
    }
}
