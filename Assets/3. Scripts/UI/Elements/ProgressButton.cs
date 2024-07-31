using System;
using _3._Scripts.Config;
using _3._Scripts.Currency.Enums;
using _3._Scripts.Stages;
using _3._Scripts.UI.Effects;
using _3._Scripts.UI.Extensions;
using _3._Scripts.UI.Panels;
using _3._Scripts.Wallet;
using DG.Tweening;
using GBGamesPlugin;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using VInspector;

namespace _3._Scripts.UI.Elements
{
    public class ProgressButton : MonoBehaviour
    {
        [SerializeField] private Image currencyImage;
        [SerializeField] private TMP_Text rewardText;
        [SerializeField] private Transform tutorial;
        [Tab("Rewards")] [SerializeField] private CurrencyType rewardType;
        [SerializeField] private CurrencyCounterEffect effect;

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
        private float _reward;

        private void Awake()
        {
            _button = GetComponent<Button>();
        }

        private void Start()
        {
            _button.onClick.AddListener(GetReward);
        }

        public void Initialize(float rewardCount)
        {
            _reward = rewardCount;
            currencyImage.sprite = Configuration.Instance.GetCurrency(rewardType).Icon;
            currencyImage.ScaleImage();
            rewardText.text = WalletManager.ConvertToWallet((decimal) rewardCount);
        }

        private void DoAnimation(bool state)
        {
            if (state)
            {
                if (_currentTween != null) return;

                _currentTween = transform.DOScale(1.2f, 0.4f)
                    .SetLoops(-1, LoopType.Yoyo)
                    .SetEase(Ease.InOutQuad);
            }
            else
            {
                _currentTween?.Kill();
                _currentTween = null;
                transform.DOScale(1, 0.1f);
            }
        }

        private void GetReward()
        {
            var effectInstance = CurrencyEffectPanel.Instance.SpawnEffect(effect, rewardType, _reward);
            effectInstance.Initialize(rewardType, _reward);

            GBGames.saves.achievementSaves.Update("cups_1", _reward);
            GBGames.saves.achievementSaves.Update("cups_100", _reward);
            GBGames.saves.achievementSaves.Update("cups_1000", _reward);

            StageController.Instance.CurrentStage.RespawnGrassFields();

            if (GBGames.saves.tutorialComplete) return;

            if (tutorial != null)
                tutorial.gameObject.SetActive(false);
            GBGames.saves.tutorialComplete = true;
        }

        public void RemoveAllListeners() => _button.onClick.RemoveAllListeners();
    }
}