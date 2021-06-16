using System;
using Game.Scripts.Infra;
using Game.Scripts.Model;
using Game.Scripts.Model.Vfx;
using UnityEngine;

namespace Game.Scripts.Level
{
    public class Cube : MonoBehaviour
    {
        private const int MaxDelayBetweenHits = 60 * 60 * 100;
        [SerializeField] private int _life = 1;
        [SerializeField] private int _layer;

        private int _coins;
        public int Coins => _coins;
        public int Layer => _layer;

        private Shaker _shaker;
        private float _lastHit;
        private float _delayBetweenHits;
        private int _touchClearTimer;
        
        public event Action<Cube> OnKill;

        private void Start()
        {
            _shaker = GetComponent<Shaker>();
        }

        private void Update()
        {
            if (_touchClearTimer <= 0)
            {
                _lastHit = -MaxDelayBetweenHits;
                return;
            }

            HandleHit();

            _touchClearTimer--;
        }
        
        public void Init(int life = 1, int coins = 0, float delayBetweenHits = MaxDelayBetweenHits)
        {
            _life = life;
            _coins = coins;
            _delayBetweenHits = delayBetweenHits;
            _lastHit = -MaxDelayBetweenHits;
        }

        public void Hit(int layer)
        {
            if (_layer != layer)
            {
                return;
            }
            _touchClearTimer = 2;
        }

        public void ForceKill()
        {
            _life = 0;
            Vibrator.Pop();
            Kill();
        }

        private void HandleHit()
        {
            var dt = Time.time - _lastHit;
            if (dt < _delayBetweenHits)
            {
                //Debug.Log("ignore hit cube: " + this);
                return;
            }

            //Debug.Log("hit cube: " + this + " , life: " + _life);
            _lastHit = Time.time;
            _life--;
            if (_life <= 0)
            {
                Vibrator.Pop();
                Kill();
                return;
            }
            else
            {
                Vibrator.Blocked();
                //_shaker.StopShake();
                //_shaker.StartShake(0.1f);
                
                ModelManager.Get().Vfx.Create(VfxType.Dust, transform.position, 2);
            }
        }

        private void Kill()
        {
            OnKill?.Invoke(this);

            var vfxType = VfxType.Pop;
            switch (_layer)
            {
                case 1 : vfxType = VfxType.PopLayer1;
                    break;
                case 2 : vfxType = VfxType.PopLayer2;
                    break;
                case 3 : vfxType = VfxType.PopLayer2;
                    break;
            }
            ModelManager.Get().Vfx.Create(vfxType, transform.position, 2);

            if (_coins > 0)
            {
                ModelManager.Get().Vfx.Create(VfxType.Coins, transform.position, 4);
            }

            Destroy(gameObject);
        }
    }
}
