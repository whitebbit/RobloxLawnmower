using _3._Scripts.Characters;
using _3._Scripts.Wallet;
using UnityEngine;
using VInspector;

namespace _3._Scripts.UI.Scriptable.Shop
{
    [CreateAssetMenu(fileName = "UpgradeItem", menuName = "Shop Items/Upgrade Item", order = 0)]
    public class UpgradeItem : ShopItem
    {
        [SerializeField] private float booster;
        
        public float Booster => booster;

        public override string Title()
        {
            return $"x{WalletManager.ConvertToWallet((decimal) booster)}<sprite index=1>";
        }
    }
}