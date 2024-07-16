using System;
using System.Linq;
using _3._Scripts.Characters;
using _3._Scripts.Config;
using Unity.VisualScripting;
using UnityEngine;
using Object = UnityEngine.Object;

namespace _3._Scripts.Upgrades
{
    public class UpgradeHandler
    {
        private readonly CharacterHandler _characterHandler;

        public UpgradeHandler(CharacterHandler characterHandler)
        {
            _characterHandler = characterHandler;
        }
        
        public void SetUpgrade(string id)
        {
            var hand = Configuration.Instance.AllUpgrades.FirstOrDefault(u => u.ID == id);
            if (hand is null) return;
            _characterHandler.Current.SetUpgrade(hand);
        }
    }
}