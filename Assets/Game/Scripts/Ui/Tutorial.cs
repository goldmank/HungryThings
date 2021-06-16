using Game.Scripts.Infra;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.Ui
{
    public class Tutorial : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private Sprite[] _frames;

        private float _timer;
        private int _frame;
        
        private void Start()
        {
            _image.sprite = _frames[0];
            Utils.DoPumpAnimation(_text.transform, 1, -1, 0.3f, 1, 1.1f);
        }

        private void Update()
        {
            _timer += Time.deltaTime;
            if (_timer > 0.3f)
            {
                _timer = 0;
                _frame = (_frame + 1) % _frames.Length;
                _image.sprite = _frames[_frame];
            }

            if (Input.GetMouseButtonUp(0))
            {
                gameObject.SetActive(false);
            }
        }
    }
}