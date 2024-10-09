using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _3._Scripts.Config;
using _3._Scripts.Interactive.Interfaces;
using _3._Scripts.UI.Enums;
using _3._Scripts.UI.Scriptable.Shop;
using _3._Scripts.Wallet;
using GBGamesPlugin;
using UnityEngine;

namespace _3._Scripts.Interactive
{
    public class AdSkinUnlocker : MonoBehaviour, IInteractive
    {
        [SerializeField] private List<SkinnedMeshRenderer> skin = new();

        private CharacterItem _characterItem;

        private void Start()
        {
            Initialize();
        }

        private void OnEnable()
        {
            WalletManager.OnSecondCurrencyChange += (_, _) => StartCoroutine(DelayInitialize());
        }

        private void Initialize()
        {
            _characterItem = GetNextCharacter();
            SetSkin();
        }

        private void SetSkin()
        {
            if (_characterItem == null) return;

            foreach (var skinnedMeshRenderer in skin)
            {
                skinnedMeshRenderer.material = _characterItem.Skin;
            }
        }


        public void StartInteract()
        {
        }

        public void Interact()
        {
            GBGames.ShowRewarded(() =>
            {
                var player = Player.Player.instance;

                player.CharacterHandler.SetCharacter(_characterItem.ID);
                player.InitializeUpgrade();

                GBGames.saves.characterSaves.Unlock(_characterItem.ID);
                GBGames.saves.characterSaves.SetCurrent(_characterItem.ID);
                GBGames.instance.Save();


                Initialize();
            });
        }

        public void StopInteract()
        {
        }

        private IEnumerator DelayInitialize()
        {
            yield return null;
            Initialize();
        }

        private CharacterItem GetNextCharacter()
        {
            var list = Configuration.Instance.AllCharacters.ToList();
            var current = list.FirstOrDefault(u => GBGames.saves.characterSaves.IsCurrent(u.ID));
            var nextItem = list.FirstOrDefault(c => current is not null && c.Booster > current.Booster);
            return nextItem;
        }
    }
}