using System;
using _3._Scripts.Config;
using _3._Scripts.Currency.Enums;
using _3._Scripts.UI.Effects;
using _3._Scripts.UI.Extensions;
using _3._Scripts.UI.Panels;
using _3._Scripts.Wallet;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using VInspector;

namespace _3._Scripts.UI.Elements
{
    public class ProgressButton: MonoBehaviour
    {
       
        [SerializeField] private CurrencyType rewardType;
        [SerializeField] private Image currencyImage;
        [SerializeField] private TMP_Text rewardText;
        public bool Interactable
        {
            get => _button.interactable;
            set => _button.interactable = value;
        }
        
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

        public void AddListener(UnityAction action) => _button.onClick.AddListener(action);
        public void RemoveAllListeners() => _button.onClick.RemoveAllListeners();
    }
}