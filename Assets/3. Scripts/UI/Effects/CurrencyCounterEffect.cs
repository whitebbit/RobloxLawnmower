using _3._Scripts.Config;
using _3._Scripts.Currency.Enums;
using _3._Scripts.UI.Extensions;
using _3._Scripts.Wallet;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _3._Scripts.UI.Effects
{
    public class CurrencyCounterEffect : UIEffect
    {
        [SerializeField] private CanvasGroup canvasGroup;

        [SerializeField] private TMP_Text counter;
        [SerializeField] private Image icon;
        

        public void Initialize(CurrencyType type, float count)
        {
            var image = Configuration.Instance.GetCurrency(type).Icon;
            icon.sprite = image;
            icon.ScaleImage();
            counter.text = $"+{WalletManager.ConvertToWallet((decimal) count)}";
            canvasGroup.alpha = 0;

            canvasGroup.DOFade(1, 0.25f).SetLink(gameObject);
        }

    }
}