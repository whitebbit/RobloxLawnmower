using System;
using System.Collections.Generic;
using System.Linq;
using _3._Scripts.Config;
using _3._Scripts.Currency.Enums;
using _3._Scripts.Currency.Scriptable;
using _3._Scripts.Stages;
using _3._Scripts.Wallet;
using UnityEngine;
using UnityEngine.Serialization;
using VInspector;

namespace _3._Scripts.UI.Scriptable.Roulette
{
    [CreateAssetMenu(fileName = "CurrencyRouletteItem", menuName = "ScriptableObjects/RouletteItem/Currency", order = 0)]
    public class CurrencyGiftItem : GiftItem
    {
        [Tab("Base settings")] [SerializeField]
        private CurrencyType type;

        [SerializeField] private int count;

        private float Count => StageController.Instance.CurrentStage == null
            ? count * 1
            : count * StageController.Instance.CurrentStage.GiftBooster;

        public override Sprite Icon()
        {
            return Configuration.Instance.GetCurrency(type)?.Icon;
        }

        public override string Title()
        {
            return WalletManager.ConvertToWallet((decimal) Count);
        }

        public override void OnReward()
        {
            WalletManager.EarnByType(type, count);
        }
    }
}