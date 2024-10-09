using System;
using System.Collections.Generic;
using System.Linq;
using _3._Scripts.Config;
using _3._Scripts.Pets.Scriptables;
using _3._Scripts.Saves;
using _3._Scripts.UI.Extensions;
using _3._Scripts.UI.Structs;
using DG.Tweening;
using GBGamesPlugin;
using UnityEngine;
using UnityEngine.UI;
using VInspector;

namespace _3._Scripts.UI.Elements
{
    public class PetSlot : MonoBehaviour
    {

        [SerializeField] private Image icon;
        [SerializeField] private Image table;
        [SerializeField] private Image equipped;
        [SerializeField] private Image selected;

        public PetSaveData SaveData { get; private set; }
        private Button _button;

        private void Awake()
        {
            _button = GetComponent<Button>();
        }

        public void Initialize(PetSaveData petSaveData)
        {
            var data = Configuration.Instance.GetPet(petSaveData.dataID);
            var rarity = Configuration.Instance.GetRarityTable(data.Rarity);


            SaveData = petSaveData;
            icon.gameObject.SetActive(true);

            if (data == null) return;

            icon.sprite = data.Icon;
            table.color = rarity.MainColor;

            Unselect();
            SetEquippedState();
        }

        private void SetEquippedState()
        {
            if (equipped == null) return;

            var select = GBGames.saves.petsSave.Selected(SaveData.id);
            if (select)
                Equip();
            else
                UnEquip();
        }

        public void SetAction(Action<PetSlot> action)
        {
            _button.onClick.AddListener(() => action?.Invoke(this));
        }

        public void Equip()
        {
            if (equipped == null) return;
            equipped.Fade(1);
        }

        public void Select()
        {
            if (selected == null) return;

            selected.Fade(1);
        }

        public void Unselect()
        {
            if (selected == null) return;

            selected.Fade(0);
        }

        public void UnEquip()
        {
            if (equipped == null) return;

            equipped.Fade(0);
        }

        public void Clear()
        {
            icon.gameObject.SetActive(false);
        }
    }
}