using System;
using _3._Scripts.Ads;
using _3._Scripts.Config;
using _3._Scripts.Localization;
using _3._Scripts.Pets.Scriptables;
using _3._Scripts.UI.Enums;
using _3._Scripts.UI.Panels.Base;
using _3._Scripts.UI.Scriptable.Shop;
using DG.Tweening;
using GBGamesPlugin;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace _3._Scripts.UI.Panels
{
    public class OfferPanel : SimplePanel
    {
        [SerializeField] private Image icon;
        [SerializeField] private Image table;
        [SerializeField] private TMP_Text boosterText;
        [SerializeField] private LocalizeStringEvent titleText;
        [SerializeField] private LocalizeStringEvent rarityText;
        [SerializeField] private Button rewardButton;
        [SerializeField] private Button closeButton;

        public override void Initialize()
        {
            base.Initialize();
            closeButton.onClick.AddListener(() =>
            {
                Debug.Log("close");
                Enabled = false;
            });
        }

        public void SetOffer<T>(T data, Action onReward) where T : ShopItem
        {
            Clear();
            SetVariables(data);
            rewardButton.onClick.AddListener(() => GBGames.ShowRewarded(() =>
            {
                onReward?.Invoke();
                Enabled = false;
            }));
            closeButton.image.DOFade(0, 0.25f).From().SetDelay(3f)
                .OnStart(() => closeButton.gameObject.SetActive(true));
        }

        public void SetIconColor(Color color) => icon.color = color;

        public void SetRarity(Rarity rarity)
        {
            var rarityTable = Configuration.Instance.GetRarityTable(rarity);
            rarityText.SetReference(rarityTable.TitleID);
            table.color = rarityTable.MainColor;
            rarityText.gameObject.SetActive(true);
        }

        public void SetBoosterText(string text)
        {
            boosterText.text = text;
            boosterText.gameObject.SetActive(true);
        }
        
        private void SetVariables<T>(T data) where T : ShopItem
        {
            icon.sprite = data.Icon;
            titleText.SetReference(data switch
            {
                PetData => "offer_pet",
                UpgradeItem => "offer_upgrade",
                TrailItem => "offer_trail",
                _ => ""
            });
        }

        private void Clear()
        {
            icon.color = Color.white;
            table.color = Color.white;
            
            rarityText.gameObject.SetActive(false);
            closeButton.gameObject.SetActive(false);
            boosterText.gameObject.SetActive(false);
            
            rewardButton.onClick.RemoveAllListeners();
        }
    }
}