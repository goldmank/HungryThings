using DG.Tweening;
using Game.Scripts.Infra;
using TMPro;
using UnityEngine;

namespace Game.Scripts.Ui
{
    public class CoinFloatReward : MonoBehaviour
    {
        public float Speed = 1;
        public float Offset = 10;
        
        [SerializeField] private TextMeshProUGUI _text;

        public void Show(int amount, Vector3 uiPos)
        {
            gameObject.SetActive(true);
            
            _text.text = "+" + amount;
            Utils.SetAlpha(_text, 1);
            
            transform.position = uiPos;
            transform.DOMoveY(uiPos.y + Offset, Speed).SetEase(Ease.OutCubic).OnComplete(() =>
            {
                Destroy(gameObject);
            });
            _text.DOFade(0, Speed*0.5f).SetDelay(Speed*0.4f);
        }
    }
}