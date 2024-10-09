using System;
using System.Collections.Generic;
using System.Linq;
using _3._Scripts.Config;
using _3._Scripts.UI.Elements.ShopSlots;
using _3._Scripts.UI.Scriptable.Shop;
using _3._Scripts.Wallet;
using GBGamesPlugin;

namespace _3._Scripts.UI.Panels
{
    public class CharacterShop : ShopPanel<CharacterItem, CharacterShopSlot>
    {
        protected override IEnumerable<CharacterItem> ShopItems()
        {
            return Configuration.Instance.AllCharacters.OrderBy(obj => obj.Booster);
        }

        protected override bool ItemUnlocked(string id)
        {
            return GBGames.saves.characterSaves.Unlocked(id);
        }

        protected override bool IsSelected(string id)
        {
            return GBGames.saves.characterSaves.IsCurrent(id);
        }

        protected override bool Select(string id)
        {
            if (!ItemUnlocked(id)) return false;
            if (IsSelected(id)) return false;

            var player = Player.Player.instance;
            player.CharacterHandler.SetCharacter(id);
            player.InitializeUpgrade();

            GBGames.saves.characterSaves.SetCurrent(id);
            SetSlotsState();
            GBGames.instance.Save();

            return true;
        }

        protected override bool Buy(string id)
        {
            if (ItemUnlocked(id)) return false;

            var slot = GetSlot(id);
            var data = slot.Data;

            return slot.ItsRewardSkin() ? AdBuy(data) : CurrencyBuy(data);
        }

        private bool CurrencyBuy(ShopItem slot)
        {
            if (!WalletManager.TrySpend(slot.CurrencyType, slot.Price)) return false;

            GBGames.saves.characterSaves.Unlock(slot.ID);
            Select(slot.ID);
            return true;
        }

        private bool AdBuy(ShopItem slot)
        {
            GBGames.ShowRewarded(() =>
            {
                SetSlotsState();
                GBGames.saves.characterSaves.Unlock(slot.ID);
                Select(slot.ID);
            });
            
            return true;
        }
    }
}