using UnityEngine;

namespace Game.Scripts.Level
{
    public class CameraMover : MonoBehaviour
    {
        [SerializeField] private Transform _camera;
        [SerializeField] private float _speed;
        [SerializeField] private float _acc;

        private Vector3? _prevInputPos;
        private Vector2 _curr;
    
        void LateUpdate()
        {
            _camera.transform.LookAt(new Vector3(0,0,0));
            _camera.transform.RotateAround(new Vector3(0,0,0), new Vector3(0.0f,1.0f,0.0f), _curr.x);
            _camera.transform.RotateAround(new Vector3(0,0,0), new Vector3(1.0f,0.0f,0.0f), _curr.y);

            _curr = Vector2.Lerp(_curr, Vector2.zero, Time.deltaTime * _acc);
        
            if (!Input.GetMouseButton(0))
            {
                _prevInputPos = null;
                return;
            }

            if (null == _prevInputPos)
            {
                _prevInputPos = Input.mousePosition;
            }

            var d = (Input.mousePosition - _prevInputPos).Value;
            _prevInputPos = Input.mousePosition;

            _curr.x += _speed * d.x;
            _curr.y += _speed * d.y;
        }
    }
}
