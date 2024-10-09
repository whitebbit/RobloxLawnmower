using System.Collections.Generic;
using System.Linq;
using _3._Scripts.Config;
using _3._Scripts.Pets;
using _3._Scripts.Saves;
using _3._Scripts.UI.Elements;
using _3._Scripts.UI.Enums;
using _3._Scripts.UI.Panels.Base;
using _3._Scripts.Wallet;
using GBGamesPlugin;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VInspector;

namespace _3._Scripts.UI.Panels
{
    public class PetsPanel : SimplePanel
    {
        [SerializeField] private TMP_Text counter;
        [SerializeField] private TMP_Text maxCount;
        [Tab("Slots")] [SerializeField] private PetSlot prefab;
        [SerializeField] private Transform container;
        [Tab("Selected")] [SerializeField] private PetSlot selected;
        [SerializeField] private PetBooster booster;
        [Tab("Buttons")]
        [SerializeField] private Button remove;
        [SerializeField] private Button getRandomPet;
        [SerializeField] private Button selectBest;

        private PetSlot _currentSlot;
        private readonly List<PetSlot> _slots = new();
        public void ShowOffer()
        {
            if (GBGames.saves.petsSave.MaxUnlocked(25))
            {
                NotificationPanel.Instance.ShowNotification("max_pet_unlocked");
                return;
            }
            
            var panel = UIManager.Instance.GetPanel<OfferPanel>();
            var pets = Configuration.Instance.AllPets.ToList();
            var data = pets[Random.Range(0, pets.Count)];
            var maxBooster = GBGames.saves.petsSave.GetMaxBooster();
            var currentBooster = Random.Range(maxBooster, maxBooster + 5);

            panel.Enabled = true;
            panel.SetOffer(data, () =>
            {
                GBGames.saves.petsSave.Unlock(data, currentBooster);
                PetUnlocker.SelectBest();
            });

            panel.SetRarity(Rarity.Legendary);
            panel.SetBoosterText($"+{WalletManager.ConvertToWallet((decimal) currentBooster)} <sprite index=1>");
        }
        
        private void GetRandomPet()
        {
            if (GBGames.saves.petsSave.MaxUnlocked(25))
            {
                NotificationPanel.Instance.ShowNotification("max_pet_unlocked");
               
                return;
            }
            
            GBGames.ShowRewarded(() =>
            {
                var pets = Configuration.Instance.AllPets.ToList();
                var data = pets[Random.Range(0, pets.Count)];
                var maxBooster = GBGames.saves.petsSave.GetMaxBooster();
                var currentBooster = Random.Range(maxBooster, maxBooster + 5);

                GBGames.saves.petsSave.Unlock(data, currentBooster);
                PetUnlocker.SelectBest();
            
                DeleteSlots();
                InitializeSlots();
                UpdateCount();
                EquipBest();
            });
        }
        
        public override void Initialize()
        {
            InTransition = transition;
            OutTransition = transition;

            booster.gameObject.SetActive(false);
            remove.onClick.AddListener(Remove);
            selectBest.onClick.AddListener(SelectBest);
            getRandomPet.onClick.AddListener(GetRandomPet);
        }

        protected override void OnOpen()
        {
            InitializeSlots();
            UpdateCount();
            EquipBest();
        }

        protected override void OnClose()
        {
            DeleteSlots();
        }

        private void UpdateCount()
        {
            counter.text = $"{GBGames.saves.petsSave.selected.Count}/3";
            maxCount.text = $"{GBGames.saves.petsSave.unlocked.Count}/25";
        }

        private void InitializeSlots()
        {
            var unlocked = GBGames.saves.petsSave.unlocked.OrderByDescending(p => p.booster).ToList();
            foreach (var item in unlocked)
            {
                var obj = Instantiate(prefab, container);

                obj.Initialize(item);
                obj.SetAction(OnClick);

                _slots.Add(obj);
            }
        }

        private void DeleteSlots()
        {
            foreach (var child in _slots)
            {
                Destroy(child.gameObject);
            }

            _slots.Clear();
            selected.Clear();
        }

        private void OnClick(PetSlot slot)
        {
            if (_currentSlot != null)
                _currentSlot.Unselect();

            _currentSlot = slot;

            if (_currentSlot != null)
                _currentSlot.Select();

            booster.gameObject.SetActive(true);
            selected.Initialize(slot.SaveData);
            booster.SetBooster(slot.SaveData);
        }

        private void SelectCurrent()
        {
            if (_currentSlot == null) return;

            var data = _currentSlot.SaveData;

            if (GBGames.saves.petsSave.MaxSelected(3)) return;
            if (GBGames.saves.petsSave.Selected(data.id)) return;

            Select(data, _currentSlot);
            GBGames.instance.Save();
            UpdateCount();
        }

        private void SelectBest()
        {
            var best = _slots.OrderByDescending(p => p.SaveData.booster).ToList();
            var selectedPets = new List<PetSaveData>(GBGames.saves.petsSave.selected);
            if (best.Count <= 0) return;

            foreach (var data in selectedPets)
            {
                Unselect(data);
            }

            for (var i = 0; i < 3; i++)
            {
                if (i >= best.Count) continue;
                Select(best[i].SaveData);
            }

            GBGames.instance.Save();
            UpdateCount();
        }

        private void EquipBest()
        {
            var best = GBGames.saves.petsSave.unlocked.OrderByDescending(p => p.booster).ToList();
            
            foreach (var slot in _slots)
            {
                slot.UnEquip();
            }
            
            for (var i = 0; i < 3; i++)
            {
                if (best.Count == i) break;

                var slot = _slots.FirstOrDefault(s => s.SaveData.id == best[i].id);
                
                if (slot == null) continue;
                
                slot.Equip();
            }
        }
        
        public void Select(PetSaveData data, PetSlot slot = null)
        {
            slot = slot == null ? _slots.FirstOrDefault(s => s.SaveData.id == data.id) : slot;

            if (slot == null) return;

            slot.Equip();

            var player = Player.Player.instance.transform;
            var position = player.position + player.right * 2;

            Player.Player.instance.PetsHandler.CreatePet(data, position);
            GBGames.saves.petsSave.Select(data.id);
        }

        private void Unselect(PetSaveData data, PetSlot slot = null)
        {
            slot = slot == null ? _slots.FirstOrDefault(s => s.SaveData.id == data.id) : slot;

            if (slot == null) return;

            slot.UnEquip();
            Player.Player.instance.PetsHandler.DestroyPet(data.id);
            GBGames.saves.petsSave.Unselect(data.id);
        }

        private void UnselectCurrent()
        {
            if (_currentSlot == null) return;

            var data = _currentSlot.SaveData;

            if (!GBGames.saves.petsSave.Selected(data.id)) return;

            Unselect(data, _currentSlot);
            GBGames.instance.Save();
            UpdateCount();
        }

        private void Remove()
        {
            var best = GBGames.saves.petsSave.unlocked.OrderByDescending(p => p.booster).ToList();
            for (var i = 0; i < best.Count; i++)
            {
                if (i < 3) continue;
                Player.Player.instance.PetsHandler.DestroyPet(best[i].id);
                GBGames.saves.petsSave.Unselect(best[i].id);
                GBGames.saves.petsSave.Remove(best[i].id);
            }
            
            GBGames.instance.Save();

            selected.Clear();
            booster.gameObject.SetActive(false);
            
            DeleteSlots();
            InitializeSlots();
            UpdateCount();
        }
    }
}