using System;
using _3._Scripts.Actions.Interfaces;
using _3._Scripts.Actions.Scriptable;
using _3._Scripts.Config;
using _3._Scripts.Currency.Enums;
using _3._Scripts.Localization;
using _3._Scripts.UI.Effects;
using _3._Scripts.UI.Panels;
using _3._Scripts.Wallet;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Localization.Components;
using VInspector;

namespace _3._Scripts.Actions
{
    public class Training: MonoBehaviour, IActionable
    {
        
        [Tab("Settings")]
        [SerializeField] private CurrencyType currencyType;
        [SerializeField] private CurrencyCounterEffect effect;
        [SerializeField] private Transform shakeObject;
        [SerializeField] private MeshRenderer mesh;
        
        [SerializeField] private ParticleSystem particle;
        [Tab("Count")]
        [SerializeField] private LocalizeStringEvent countText;
        [Tab("Required")]
        [SerializeField] private LocalizeStringEvent requiredText;

        private float _requiredCount;
        private float _count;
        private Vector3 _startPosition;
        public void Initialize(TrainingConfig config)
        {
            _startPosition = shakeObject.localPosition;
            _count = config.Count;
            _requiredCount = config.RequiredCount;
            
            mesh.materials[0].DOColor(config.Color, 0);
            requiredText.SetVariable("value", WalletManager.ConvertToWallet((decimal) _requiredCount));
            countText.SetVariable("value", WalletManager.ConvertToWallet((decimal) _count));
        }

        public void Action()
        {
            if(WalletManager.GetQuantityByType(currencyType) < _requiredCount) return;
            
            var training = Player.Player.instance.GetTrainingStrength(_count);
            var obj = CurrencyEffectPanel.Instance.SpawnEffect(effect, currencyType, training);
            
            obj.Initialize(currencyType, training);
            shakeObject.DOShakePosition(0.25f, 0.25f, 50).OnComplete(() => shakeObject.localPosition = _startPosition);
            particle.Play();
        }
    }
}