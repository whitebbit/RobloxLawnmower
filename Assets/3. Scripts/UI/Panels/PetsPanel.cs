using System.Collections.Generic;
using System.Linq;
using _3._Scripts.Config;
using _3._Scripts.Pets.Scriptables;
using _3._Scripts.Saves;
using _3._Scripts.UI.Elements;
using _3._Scripts.UI.Panels.Base;
using GBGamesPlugin;
using TMPro;
using Unity.VisualScripting;
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
        [Tab("Buttons")] [SerializeField] private Button select;
        [SerializeField] private Button unselect;
        [SerializeField] private Button remove;
        [SerializeField] private Button selectBest;

        private PetSlot _currentSlot;
        private readonly List<PetSlot> _slots = new();

        public override void Initialize()
        {
            InTransition = transition;
            OutTransition = transition;

            booster.gameObject.SetActive(false);
            select.onClick.AddListener(SelectCurrent);
            unselect.onClick.AddListener(UnselectCurrent);
            remove.onClick.AddListener(Remove);
            selectBest.onClick.AddListener(SelectBest);
        }

        protected override void OnOpen()
        {
            InitializeSlots();
            UpdateCount();
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
            var unlocked = GBGames.saves.petsSave.unlocked;
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
            if(_currentSlot != null)
                _currentSlot.Unselect();
            
            _currentSlot = slot;
            
            if(_currentSlot != null)
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
            if(best.Count <= 0) return;
             
            foreach (var data in selectedPets)
            {
                Unselect(data);
            }

            for (var i = 0; i < 3; i++)
            {
                if(i >= best.Count) continue;
                Select(best[i].SaveData);
            }

            GBGames.instance.Save();
            UpdateCount();
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
            if (_currentSlot == null) return;
            var data = _currentSlot.SaveData;

            _currentSlot.UnEquip();

            Player.Player.instance.PetsHandler.DestroyPet(data.id);
            GBGames.saves.petsSave.Unselect(data.id);
            GBGames.saves.petsSave.Remove(data.id);
            GBGames.instance.Save();

            selected.Clear();
            booster.gameObject.SetActive(false);

            _slots.Remove(_currentSlot);
            Destroy(_currentSlot.gameObject);
            UpdateCount();
        }
    }
}