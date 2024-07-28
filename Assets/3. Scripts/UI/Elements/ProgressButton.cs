using System;
using _3._Scripts.Config;
using _3._Scripts.Currency.Enums;
using _3._Scripts.UI.Effects;
using _3._Scripts.UI.Extensions;
using _3._Scripts.UI.Panels;
using _3._Scripts.Wallet;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using VInspector;

namespace _3._Scripts.UI.Elements
{
    public class ProgressButton : MonoBehaviour
    {
        [SerializeField] private CurrencyType rewardType;
        [SerializeField] private Image currencyImage;
        [SerializeField] private TMP_Text rewardText;

        public bool Interactable
        {
            get => _button.interactable;
            set
            {
                DoAnimation(value);
                _button.interactable = value;
            }
        }

        private Tween _currentTween;

        private Button _button;

        private void Awake()
        {
            _button = GetComponent<Button>();
        }

        public void Initialize(float rewardCount)
        {
            currencyImage.sprite = Configuration.Instance.GetCurrency(rewardType).Icon;
            currencyImage.ScaleImage();
            rewardText.text = WalletManager.ConvertToWallet((decimal) rewardCount);
        }

        private void DoAnimation(bool state)
        {
            if (state)
            {
                if (_currentTween != null) return;
                
                _currentTween = transform.DOScale(1.1f, 0.4f)
                    .SetLoops(-1, LoopType.Yoyo)
                    .SetEase(Ease.InOutQuad);
            }
            else
            {
                _currentTween?.Kill();
                transform.DOScale(1, 0.1f);
            }
        }

        public void AddListener(UnityAction action) => _button.onClick.AddListener(action);
        public void RemoveAllListeners() => _button.onClick.RemoveAllListeners();
    }
}