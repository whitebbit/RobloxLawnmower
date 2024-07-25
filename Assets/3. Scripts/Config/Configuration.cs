using System;
using System.Collections.Generic;
using System.Linq;
using _3._Scripts.Achievements.Scriptables;
using _3._Scripts.Characters;
using _3._Scripts.Currency.Enums;
using _3._Scripts.Currency.Scriptable;
using _3._Scripts.Pets.Scriptables;
using _3._Scripts.Player.Scriptables;
using _3._Scripts.Singleton;
using _3._Scripts.Sounds.Scriptable;
using _3._Scripts.UI.Scriptable.Shop;
using GBGamesPlugin;
using UnityEngine;
using UnityEngine.Serialization;
using VInspector;

namespace _3._Scripts.Config
{
    public class Configuration : Singleton<Configuration>
    {
        [Tab("Remote")] [SerializeField]
        private RemoteConfig<bool> interByTime;

        
        [Tab("Upgrades")]
        [SerializeField] private List<CharacterItem> allCharacters = new();
        [SerializeField] private List<PetData> allPets = new();
        [SerializeField] private List<LawnmowerData> allLawnmower = new();
        [Tab("Other")]
        [SerializeField] private List<CurrencyData> currencyData = new();

        [SerializeField] private List<AchievementData> achievementData = new();
        [Tab("Unused")]
        [SerializeField] private List<TrailItem> allTrails = new();

        public bool InterByTime => interByTime.Value;
        private IEnumerable<PetData> AllPets => allPets;
        public IEnumerable<AchievementData> AchievementData => achievementData;

        public IEnumerable<LawnmowerData> AllLawnmower => allLawnmower;
        public IEnumerable<CharacterItem> AllCharacters => allCharacters;

        public IEnumerable<TrailItem> AllTrails => allTrails;
        public CurrencyData GetCurrency(CurrencyType type) => currencyData.FirstOrDefault(c => c.Type == type);
        public PetData GetPet(string id) => AllPets.FirstOrDefault(p => p.ID == id);

        private void Start()
        {
            GBGames.InGameLoadingStopped();
            GBGames.GameplayStarted();
            GBGames.GameReady();
            
        }
    }
}