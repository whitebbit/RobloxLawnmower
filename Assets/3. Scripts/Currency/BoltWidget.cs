using System;
using System.Linq;
using _3._Scripts.Config;
using _3._Scripts.Currency.Enums;
using _3._Scripts.Localization;
using _3._Scripts.Player.Scriptables;
using _3._Scripts.Wallet;
using GBGamesPlugin;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace _3._Scripts.Currency
{
    public class BoltWidget : MonoBehaviour
    {
        [SerializeField] private TMP_Text countToUnlock;
        [SerializeField] private Slider progressSlider;
        [SerializeField] private LocalizeStringEvent levelText;
        [SerializeField] private TMP_Text upgradeText;
        [SerializeField] private Button unlockButton;

        private void Start()
        {
            unlockButton.onClick.AddListener(OnUnlockButtonClicked);
        }

        private void OnEnable()
        {
            UpdateUI(WalletManager.ThirdCurrency);
            WalletManager.OnThirdCurrencyChange += OnCurrencyChange;
        }

        private void OnDisable()
        {
            WalletManager.OnThirdCurrencyChange -= OnCurrencyChange;
        }

        private void OnCurrencyChange(float _, float newCurrencyAmount)
        {
            UpdateUI(newCurrencyAmount);
        }

        private void UpdateUI(float currentCurrency)
        {
            var currentLawnmower = GetLawnmowerAtLevel(GBGames.saves.lawnmowerLevel);
            var nextLawnmower = GetLawnmowerAtLevel(GBGames.saves.lawnmowerLevel + 1);

            if (currentLawnmower == null || nextLawnmower == null)
            {
                gameObject.SetActive(false);
                return;
            }

            var canUnlock = currentCurrency >= nextLawnmower.BoltsToUnlock;

            countToUnlock.gameObject.SetActive(!canUnlock);
            progressSlider.gameObject.SetActive(!canUnlock);
            unlockButton.gameObject.SetActive(canUnlock);

            levelText.SetVariable("level_1", currentLawnmower.Level.ToString());
            levelText.SetVariable("level_2", nextLawnmower.Level.ToString());
            upgradeText.text = $"{currentLawnmower.CupsBooster}X > <color=green>{nextLawnmower.CupsBooster}X</color>";
            countToUnlock.text = $"<sprite index=3>{currentCurrency}/{nextLawnmower.BoltsToUnlock}";
            progressSlider.value = currentCurrency / nextLawnmower.BoltsToUnlock;
        }

        private void OnUnlockButtonClicked()
        {
            var nextLawnmower = GetLawnmowerAtLevel(GBGames.saves.lawnmowerLevel + 1);

            if (nextLawnmower == null)
                return;

            Player.Player.instance.UpgradeLawnmower();
            WalletManager.ThirdCurrency -= nextLawnmower.BoltsToUnlock;
        }

        private LawnmowerData GetLawnmowerAtLevel(int level)
        {
            return Configuration.Instance.AllLawnmower.FirstOrDefault(l => l.Level == level);
        }
    }
}