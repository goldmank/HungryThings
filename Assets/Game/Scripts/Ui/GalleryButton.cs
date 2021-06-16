using System;
using Game.Scripts.Infra;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.Ui
{
    public class GalleryButton : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private Image _hiddenObject;
        [SerializeField] private GameObject _pricePanel;
        [SerializeField] private GameObject _selectPanel;
        [SerializeField] private GameObject _rvPanel;
        [SerializeField] private TextMeshProUGUI _price;
        [SerializeField] private TextMeshProUGUI _selectText;
        [SerializeField] private TextMeshProUGUI _rvText;
        [SerializeField] private Image _bg;
        [SerializeField] private Color[] _bgColors;
        
        public Action<GalleryButton> Clicked;

        public object Data;
        
        public void OnClick()
        {
            Clicked?.Invoke(this);
        }

        public void SetIcon(Sprite icon)
        {
            _icon.sprite = icon;
            _hiddenObject.sprite = icon;
        }

        public void ShowHiddenIcon(bool hidden)
        {
            _icon.gameObject.SetActive(!hidden);
            _hiddenObject.gameObject.SetActive(hidden);
        }

        public void SetHiddenColor(Material color)
        {
            _hiddenObject.material = color;
        }

        public bool IsSelectable()
        {
            return _selectPanel.gameObject.activeSelf;
        }
        
        public void ShowPrice(int price)
        {
            _rvPanel.gameObject.SetActive(false);
            
            if (price <= 0)
            {
                _pricePanel.gameObject.SetActive(false);
                _selectPanel.gameObject.SetActive(true);
            }
            else
            {
                _pricePanel.gameObject.SetActive(true);
                _selectPanel.gameObject.SetActive(false);
            }
            _price.text = price.ToString();
        }

        public void ShowRv(int curr, int total)
        {
            _pricePanel.gameObject.SetActive(false);
            
            if (curr < total)
            {
                _rvPanel.gameObject.SetActive(true);
                _rvText.text = curr + "/" + total;
                _selectPanel.gameObject.SetActive(false);
            }
            else
            {
                _rvPanel.gameObject.SetActive(false);
                _selectPanel.gameObject.SetActive(true);
            }
        }

        public void ShowLabel(string text)
        {
            _pricePanel.gameObject.SetActive(false);
            if (null != _rvPanel)
            {
                _rvPanel.gameObject.SetActive(false);   
            }
            _selectPanel.gameObject.SetActive(true);
            _selectText.text = text;
        }

        public void SetSelected(bool selected)
        {
            _bg.color = _bgColors[selected ? 1 : 0];
        }
    }
}