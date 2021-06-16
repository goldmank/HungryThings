using TMPro;
using UnityEngine;

namespace Game.Scripts.Ui
{
    public class MenuButton : MonoBehaviour
    {
        [SerializeField] private GameObject _alert;
        [SerializeField] private TextMeshProUGUI _alertCounter;
        
        public void SetAlert(int count)
        {
            _alert.gameObject.SetActive(count > 0);
            _alertCounter.text = _alertCounter.ToString();
        }
    }
}