﻿using System.Collections.Generic;
using System.Linq;
using _3._Scripts.Currency.Enums;
using _3._Scripts.Interactive.Interfaces;
using _3._Scripts.Pets.Scriptables;
using _3._Scripts.UI;
using _3._Scripts.UI.Elements;
using _3._Scripts.UI.Enums;
using _3._Scripts.UI.Panels;
using _3._Scripts.Wallet;
using GBGamesPlugin;
using TMPro;
using UnityEngine;
using VInspector;

namespace _3._Scripts.Pets
{
    public class PetUnlocker : MonoBehaviour, IInteractive
    {
        [Tab("Price Settings")] [SerializeField]
        private TMP_Text priceText;

        [Tab("Interactive Settings")] [SerializeField]
        private Transform eggModel;

        [SerializeField] private Transform canvas;

        [Tab("UI Settings")] [SerializeField] private RectTransform content;
        [SerializeField] private PetUnlockerSlot slotPrefab;

        private List<PetData> _data = new();

        private float _price;

        public void Initialize(PetUnlockerConfig config)
        {
            _data = config.Pets;
            _price = config.Price;

            InitializeUI();
            PopulatePetSlots();
            SetInitialVisibility();
        }

        private void InitializeUI()
        {
            priceText.text = $"<sprite index=0>{WalletManager.ConvertToWallet((decimal) _price)}";
        }

        private void PopulatePetSlots()
        {
            if (content.childCount > 0) return;

            _data.Sort((x, y) => y.DropPercent.CompareTo(x.DropPercent));
            foreach (var petData in _data)
            {
                var petSlot = Instantiate(slotPrefab, content);
                petSlot.Initialize(petData.Icon, petData.Rarity, petData.DropPercent);
            }
        }

        private void SetInitialVisibility()
        {
            canvas.gameObject.SetActive(false);
            eggModel.gameObject.SetActive(true);
        }

        private PetData GetRandomPet()
        {
            var totalWeight = _data.Sum(d => d.DropPercent);
            var randomValue = Random.Range(0, totalWeight);
            var cumulativeWeight = 0f;

            foreach (var petData in _data)
            {
                cumulativeWeight += petData.DropPercent;
                if (randomValue <= cumulativeWeight)
                {
                    return petData;
                }
            }

            return null;
        }

        public void StartInteract()
        {
            eggModel.gameObject.SetActive(false);
            canvas.gameObject.SetActive(true);
        }

        public void Interact()
        {
            if (GBGames.saves.petsSave.MaxUnlocked(25))
            {
                NotificationPanel.Instance.ShowNotification("max_pet_unlocked");
                return;
            }

            if (!WalletManager.TrySpend(CurrencyType.Second, _price))
            {
                ShowOffer();
                return;
            }

            var panel = UIManager.Instance.GetPanel<PetUnlockerPanel>();
            if (panel.Enabled) return;

            panel.Enabled = true;
            var pet = GetRandomPet();
            panel.UnlockPet(pet);

            SelectBest();

            if (pet.Rarity == Rarity.Legendary)
                GBGames.saves.achievementSaves.Update("legendary_pet", 1);
        }

        public static void SelectBest()
        {
            var best = GBGames.saves.petsSave.unlocked.OrderByDescending(p => p.booster).ToList();
            if (best.Count <= 0) return;

            Player.Player.instance.PetsHandler.ClearPets();
            GBGames.saves.petsSave.selected.Clear();

            for (var i = 0; i < 3; i++)
            {
                if (i >= best.Count) continue;
                var player = Player.Player.instance.transform;
                var position = player.position + player.right * 2;
                Player.Player.instance.PetsHandler.CreatePet(best[i], position);
                GBGames.saves.petsSave.Select(best[i].id);
            }

            GBGames.instance.Save();
        }

        public void StopInteract()
        {
            eggModel.gameObject.SetActive(true);
            canvas.gameObject.SetActive(false);
        }

        private void ShowOffer()
        {
            var panel = UIManager.Instance.GetPanel<OfferPanel>();
            var data = _data[Random.Range(0, _data.Count)];
            var booster = data.RandomBooster;

            panel.Enabled = true;
            panel.SetOffer(data, () =>
            {
                GBGames.saves.petsSave.Unlock(data, booster);
                SelectBest();
            });

            panel.SetRarity(data.Rarity);
            panel.SetBoosterText($"+{WalletManager.ConvertToWallet(booster)} <sprite index=1>");
        }
    }
}