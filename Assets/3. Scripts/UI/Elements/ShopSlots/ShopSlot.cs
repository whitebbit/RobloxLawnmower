using System;
using System.Collections.Generic;
using System.Linq;
using _3._Scripts.Config;
using _3._Scripts.Localization;
using _3._Scripts.UI.Scriptable.Shop;
using _3._Scripts.UI.Structs;
using _3._Scripts.Wallet;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VInspector;

namespace _3._Scripts.UI.Elements.ShopSlots
{
    public class ShopSlot : BaseShopSlot
    {
        [Tab("UI")] 
        [SerializeField] private TMP_Text title;
        [SerializeField] private Image glow;
        [SerializeField] private Image icon;
        [SerializeField] private Image table;
        [SerializeField] private Image backGlow;
        
        public override void SetView(ShopItem item)
        {
            var rarity = Configuration.Instance.GetRarityTable(item.Rarity);
            table.color = rarity.MainColor;
            glow.color = rarity.MainColor;
            backGlow.color = rarity.AdditionalColor;
            icon.sprite = item.Icon;
            title.text = item.Title();
            Data = item;
        }
        public void SetIconColor(Color color) => icon.color = color;

    }
}