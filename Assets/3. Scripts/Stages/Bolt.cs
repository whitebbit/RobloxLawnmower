using System;
using _3._Scripts.Currency.Enums;
using _3._Scripts.UI.Effects;
using _3._Scripts.UI.Panels;
using _3._Scripts.Wallet;
using TMPro;
using UnityEngine;

namespace _3._Scripts.Stages
{
    public class Bolt : MonoBehaviour
    {
        [SerializeField] private float count;
        [SerializeField] private TMP_Text text;
        [SerializeField] private CurrencyCounterEffect effect;

        private void Start()
        {
            text.text = $"+{count}";
        }

        private void OnTriggerEnter(Collider other)
        {
            if(!other.TryGetComponent(out Player.Player _)) return;
            var effectInstance = CurrencyEffectPanel.Instance.SpawnEffect(effect, CurrencyType.Third, count);
            effectInstance.Initialize(CurrencyType.Third, count);
            //Destroy(gameObject);
        }
    }
}