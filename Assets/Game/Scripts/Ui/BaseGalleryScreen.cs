using System;
using System.Collections.Generic;
using Game.Scripts.Model;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.Ui
{
    public abstract class BaseGalleryScreen : MonoBehaviour
    {
        [SerializeField] private RectTransform _content;
        [SerializeField] private ScrollRect _scrollRect;
        [SerializeField] private GalleryButton _pfButton;
        
        private List<GalleryButton> _buttons;

        private Action _onClose;
        
        public void Show(Action onClose)
        {
            _onClose = onClose;
            gameObject.SetActive(true);

            if (null == _buttons || AlwaysCreate())
            {
                foreach (Transform child in _content.transform) {
                    Destroy(child.gameObject);
                }
                
                _buttons = new List<GalleryButton>();
                for (int i = 0; i < GetCount(); i++)
                {
                    var item = Instantiate(_pfButton, _content);
                    item.Clicked += OnButtonClicked;
                    _buttons.Add(item);
                }

                var itemsInRow = 3;
                var rowHeight = 490;
                var contentSize = _content.sizeDelta;
                var rowsCount = _buttons.Count / itemsInRow;
                if (_buttons.Count % itemsInRow != 0)
                {
                    rowsCount++;
                }
                contentSize.y = rowsCount * rowHeight;
                _content.sizeDelta = contentSize;
            }

            _scrollRect.normalizedPosition = new Vector2(0, 1);
            
            for (int i = 0; i < _buttons.Count; i++)
            {
                InitItem(i, _buttons[i]);
            }
        }

        public void OnCloseClicked()
        {
            ModelManager.Get().AudioManager.PlayBack();
            gameObject.SetActive(false);
            _onClose?.Invoke();
        }

        protected virtual bool AlwaysCreate()
        {
            return false;
        }
        
        protected abstract int GetCount();
        protected abstract void InitItem(int index, GalleryButton item);
        protected abstract void OnButtonClicked(GalleryButton item);
    }
}