using System;
using System.Collections.Generic;
using System.Linq;
using _3._Scripts.Boosters;
using _3._Scripts.Characters;
using _3._Scripts.Config;
using _3._Scripts.MiniGame;
using _3._Scripts.Pets;
using _3._Scripts.Player.Scriptables;
using _3._Scripts.Saves;
using _3._Scripts.Stages;
using _3._Scripts.Trails;
using _3._Scripts.Tutorial;
using _3._Scripts.UI;
using _3._Scripts.UI.Widgets;
using _3._Scripts.Upgrades;
using _3._Scripts.Wallet;
using GBGamesPlugin;
using UnityEngine;

namespace _3._Scripts.Player
{
    public class Player : Fighter
    {
        [SerializeField] private Lawnmower lawnmower;
        [SerializeField] private PlayerLevel level;
        [SerializeField] private Character character;

        public PetsHandler PetsHandler { get; private set; }
        public CharacterHandler CharacterHandler { get; private set; }
        public UpgradeHandler UpgradeHandler { get; private set; }
        public PlayerAnimator PlayerAnimator { get; private set; }

        private CharacterController _characterController;
        public PlayerMovement Movement { get; private set; }
        public static Player instance;

        private void Awake()
        {
            if (instance == null)
                instance = this;

            PlayerAnimator = GetComponent<PlayerAnimator>();
            Movement = GetComponent<PlayerMovement>();
            PetsHandler = new PetsHandler();
            CharacterHandler = new CharacterHandler(character);
            UpgradeHandler = new UpgradeHandler(CharacterHandler);
            _characterController = GetComponent<CharacterController>();
        }

        public override FighterData FighterData()
        {
            var photo =
                Configuration.Instance.AllCharacters.FirstOrDefault(c => c.ID == GBGames.saves.characterSaves.current)
                    ?.Icon;

            return new FighterData
            {
                photo = photo,
                health = BoostersHandler.Instance.GetBoosterState("health_booster") ? 200 : 100,
                strength = BoostersHandler.Instance.GetBoosterState("slap_booster")
                    ? WalletManager.FirstCurrency * 2
                    : WalletManager.FirstCurrency
            };
        }

        protected override PlayerAnimator Animator()
        {
            return PlayerAnimator;
        }

        public void SetLevelState(bool state) => level.gameObject.SetActive(state);

        public float GetTrainingStrength(float strengthPerClick)
        {
            var pets = GBGames.saves.petsSave.selected.Sum(p => p.booster);
            var character = Configuration.Instance.AllCharacters.FirstOrDefault(
                h => h.ID == GBGames.saves.characterSaves.current).Booster;

            return (strengthPerClick * (pets + character));
        }

        public void Teleport(Vector3 position)
        {
            _characterController.enabled = false;
            transform.position = position;
            _characterController.enabled = true;
        }

        public void Reborn()
        {
            WalletManager.FirstCurrency = 0;
            WalletManager.SecondCurrency = 0;

            GBGames.saves.petsSave = new PetSave();
            GBGames.saves.characterSaves = new SaveHandler<string>();
            GBGames.saves.upgradeSaves = new SaveHandler<string>();

            DefaultDataProvider.Instance.SetPlayerDefaultData();

            Initialize();
        }

        public void SetMowingState(bool state)
        {
            if (state)
            {
                UIManager.Instance.GetWidget<GrassProgressWidget>().Setup();
                if (!GBGames.saves.tutorialComplete)
                {
                    TutorialSystem.StepComplete("mowing");
                }
            }

            UIManager.Instance.GetWidget<GrassProgressWidget>().Enabled = state;

            Movement.JumpBlocked = state;
            PlayerAnimator.SetGrounded(true);
            PlayerAnimator.SetMowingState(state);
            lawnmower.gameObject.SetActive(state);
            Movement.ResetSpeed();

        }

        private void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            InitializeCharacter();
            InitializePets();
            InitializeUpgrade();
            InitializeLawnmower();
        }

        public void UpgradeLawnmower()
        {
            var data = Configuration.Instance.AllLawnmower.FirstOrDefault(l =>
                l.Level == GBGames.saves.lawnmowerLevel + 1);
            if (data == null) return;

            GBGames.saves.lawnmowerLevel += 1;
            UIManager.Instance.GetWidget<GrassProgressWidget>().Setup();
            lawnmower.Initialize(data);
        }

        private void InitializeLawnmower()
        {
            var data = Configuration.Instance.AllLawnmower.FirstOrDefault(l => l.Level == GBGames.saves.lawnmowerLevel);
            lawnmower.Initialize(data);
            SetMowingState(false);
        }

        private void Update()
        {
            if (isFight && Input.GetMouseButtonDown(0))
            {
                Slap();
            }
        }

        private void InitializeCharacter()
        {
            var id = GBGames.saves.characterSaves.current;
            CharacterHandler.SetCharacter(id);
        }

        public void InitializeUpgrade()
        {
            var id = GBGames.saves.upgradeSaves.current;
            UpgradeHandler.SetUpgrade(id);
        }

        private void InitializePets()
        {
            var player = transform;
            var position = player.position + player.right * 2;
            PetsHandler.ClearPets();
            foreach (var petSaveData in GBGames.saves.petsSave.selected)
            {
                PetsHandler.CreatePet(petSaveData, position);
            }
        }

        private void OnDestroy()
        {
            instance = null;
        }
    }
}