using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Game.Scripts.Model;
using TMPro;
using UnityEngine;

namespace Game.Scripts.Ui
{
    public class CoinsView : MonoBehaviour
    {
        private const float ChangeDuration = 0.6f;
        
        [SerializeField] private TextMeshProUGUI _coins;
        
        private TweenerCore<float, float, FloatOptions> _tweener;

        private float _currAmount;

        private void Start()
        {
            ModelManager.Get().Currency.OnCoinsAmountChanged += OnAmountChanged;
            _currAmount = ModelManager.Get().Currency.Coins;
            UpdateChangeProgress();
        }

        private void OnDestroy()
        {
            ModelManager.Get().Currency.OnCoinsAmountChanged -= OnAmountChanged;
        }

        private void OnAmountChanged(int updatedAmount)
        {
            if (_tweener != null)
            {
                _tweener.Kill();
                _tweener = null;
            }

            _tweener = DOTween.To(() => _currAmount, x => _currAmount = x, updatedAmount, ChangeDuration)
                .OnUpdate(UpdateChangeProgress).SetEase(Ease.Linear);
        }

        private void UpdateChangeProgress()
        {
            _coins.text = ((int)_currAmount).ToString();
        }
    }
}