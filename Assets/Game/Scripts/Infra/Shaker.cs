using System;
using UnityEngine;

namespace Game.Scripts
{
    public class Shaker : MonoBehaviour
    {
	    [SerializeField] private float shakeSpeed = 2.0f;
		[SerializeField] private float shakeMagnitude = 0.01f;

		private float _duration;
		private float _timer;
		private Vector2 _shakeOffset;
		private System.Random _random;
		private Vector2 _position;
		private Action _onComplete;
		private bool _enabled;
		private Vector3 _restorePos;
		
		public void StartShake(float duration, Action onComplete = null)
		{
			if (_timer > 0)
			{
				return;
			}

			_duration = duration;
			_onComplete = onComplete;
			_timer = 0.0001f;
			_position = transform.position;
			_restorePos = transform.position;
		}

		public void StopShake()
		{
			if (_timer <= 0)
			{
				return;
			}
			transform.position = _restorePos;
			
			_timer = 0;
			_duration = 0;
			_onComplete = null;
			_shakeOffset = Vector2.zero;
			_position = Vector3.zero;
		}
		
		private void Start ()
		{
			_shakeOffset = Vector2.zero;
			_random = new System.Random ();
		}

		private void Update () 
		{
			if (_timer <= 0)
			{
				return;
			}
			_timer += Time.deltaTime;
			//_position += _body.velocity * Time.deltaTime;

			Shake();
			var x = _position.x + _shakeOffset.x;
			var y = _position.y + _shakeOffset.y;
			transform.position = new Vector3 (x, y, transform.position.z);

			if (_timer >= _duration) {
				_onComplete?.Invoke();
				_onComplete = null;
				_timer = 0;
			}
		}
		
		private void Shake() {
			var randomStart = (float)(-1000 + _random.NextDouble() * 2000);

			var t = _timer - _duration;
			var currD = _duration;
			var percentComplete = t / currD;

			// We want to reduce the shake from full power to 0 starting half way through
			var damper = 1.0f - Mathf.Clamp(2.0f * percentComplete - 1.0f, 0.0f, 1.0f);

			// Calculate the noise parameter starting randomly and going as fast as speed allows
			var alpha = randomStart + shakeSpeed * percentComplete;

			// map noise to [-1, 1]
			var x = Noise((int)alpha, 0) * 2.0f - 1.0f;
			var y = Noise(0, (int)alpha) * 2.0f - 1.0f;

			x *= shakeMagnitude * damper;
			y *= shakeMagnitude * damper;
			_shakeOffset.x = x;
			_shakeOffset.y = y;
		}

		private float Noise(int x, int y) {
			int n = x + y * 57;
			n = (n << 13) ^ n;
			return (float)((1.0 - ((n * ((n * n * 15731) + 789221) + 1376312589) & 0x7fffffff) / 1073741824.0));
		}
    }
}