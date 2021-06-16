using DG.Tweening;
using UnityEngine;

namespace Game.Scripts.Level
{
    public class Tool : MonoBehaviour
    {
        [SerializeField] private Vector3 _hitOffset;
        
        public void Hit(Vector3 position)
        {
            transform.position = position + _hitOffset;
            transform.DORotate(new Vector3(-135f, -90, 0), 0.2f).OnComplete(() =>
                {
                    transform.DORotate(new Vector3(-83, -90, 0), 0.2f);
                });
        }
    }
}