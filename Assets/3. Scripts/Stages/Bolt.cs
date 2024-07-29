using System;
using _3._Scripts.Currency.Enums;
using _3._Scripts.UI.Effects;
using _3._Scripts.UI.Panels;
using _3._Scripts.Wallet;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace _3._Scripts.Stages
{
    public class Bolt : MonoBehaviour
    {
        [SerializeField] private TMP_Text text;
        [SerializeField] private CurrencyCounterEffect effect;

        private float _count;

        public void Initialize(int count)
        {
            _count = count;
            text.text = $"+{count}";
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent(out Player.Player _)) return;
            var effectInstance = CurrencyEffectPanel.Instance.SpawnEffect(effect, CurrencyType.Third, _count);
            effectInstance.Initialize(CurrencyType.Third, _count);
            transform.DOScale(0, 0.25f).SetEase(Ease.InOutBack).OnComplete(() => Destroy(gameObject));
        }
    }
}