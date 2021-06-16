using IO.Infra.Scripts.Events;
using TMPro;
using UnityEngine;

namespace Game.Scripts.Infra
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class StringBinder : MonoBehaviour
    {
        [SerializeField] private string _key;
        
        private TextMeshProUGUI _text;
        
        private void Awake()
        {
            _text = GetComponent<TextMeshProUGUI>();
            UpdateText();
            //SimpleEventManager.Get().Subscribe(Events.Ui.StringsUpdated, OnStringUpdated);
        }

        private void OnStringUpdated(EventParams obj)
        {
            UpdateText();
        }

        private void UpdateText()
        {
            if (!Strings.IsInitialized())
            {
                return;
            }
            
            var text = Strings.Get(_key);
            if (string.IsNullOrEmpty(text))
            {
                Debug.Log("* Missing Text * key=" + _key);
                return;
            }

            _text.text = text;
        }
    }
}