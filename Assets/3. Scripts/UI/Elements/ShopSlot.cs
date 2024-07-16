using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _3._Scripts.Currency.Scriptable;
using _3._Scripts.Localization;
using _3._Scripts.UI.Scriptable.Shop;
using _3._Scripts.UI.Structs;
using _3._Scripts.Wallet;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;
using UnityEngine.UI;
using VInspector;

namespace _3._Scripts.UI.Elements
{
    public class ShopSlot : MonoBehaviour
    {
        [Tab("Rarity Tables")] [SerializeField]
        private List<RarityTable> rarityTables = new();

        [Tab("UI")] [SerializeField] private TMP_Text title;
        [SerializeField] private Image glow;
        [SerializeField] private Image icon;
        [SerializeField] private Image table;
        [SerializeField] private Image backGlow;
        [Tab("Currency")]
        [SerializeField] private TMP_Text price;
        [Tab("Localization")] [SerializeField] private string selectKey;
        [SerializeField] private string selectedKey;
        
        public ShopItem Data { get; private set; }
        private Button _button;

        private void Awake()
        {
            _button = GetComponent<Button>();
        }

        public void SetView(ShopItem item)
        {
            var rarity = rarityTables.FirstOrDefault(r => r.Rarity == item.Rarity);
            table.sprite = rarity.Table;
            glow.color = rarity.MainColor;
            backGlow.color = rarity.AdditionalColor;
            icon.sprite = item.Icon;
            title.text = item.Title();
            Data = item;
        }

        public void SetAction(Action action) => _button.onClick.AddListener(() => action?.Invoke());

        public async void Select()
        {
            price.text =  await selectedKey.GetTranslate();
        }

        public async void Unselect()
        {
            price.text = await selectKey.GetTranslate();
        }

        public void Lock()
        {
            price.text = $"<sprite index=0>{WalletManager.ConvertToWallet((decimal) Data.Price)}";
        }

        public void SetIconColor(Color color) => icon.color = color;
    }
}