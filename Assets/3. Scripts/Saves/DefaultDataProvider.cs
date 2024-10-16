﻿using System;
using _3._Scripts.Singleton;
using _3._Scripts.UI.Scriptable.Shop;
using GBGamesPlugin;
using UnityEngine;

namespace _3._Scripts.Saves
{
    public class DefaultDataProvider : Singleton<DefaultDataProvider>
    {
        [SerializeField] private CharacterItem defaultCharacter;

        private void Awake()
        {
            SetDefault();
        }

        private void SetDefault()
        {
            if (GBGames.saves.defaultLoaded) return;
            
            SetPlayerDefaultData();

            GBGames.saves.defaultLoaded = true;
            GBGames.instance.Save();
        }

        public void SetPlayerDefaultData()
        {
            GBGames.saves.characterSaves.current = defaultCharacter.ID;
            GBGames.saves.characterSaves.Unlock(defaultCharacter.ID);
        }
    }
}