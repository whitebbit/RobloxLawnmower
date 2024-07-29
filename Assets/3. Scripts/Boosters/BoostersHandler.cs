using System;
using System.Collections.Generic;
using System.Linq;
using _3._Scripts.Singleton;
using _3._Scripts.Stages;
using GBGamesPlugin;
using UnityEngine;
using UnityEngine.UI;
using VInspector;

namespace _3._Scripts.Boosters
{
    public class BoostersHandler : Singleton<BoostersHandler>
    {
        [Tab("Buttons")]
        [SerializeField] private BoosterButton autoClickerButton;
        [SerializeField] private BoosterButton rewardBooster;
        [Tab("Debug")]
        [SerializeField] private List<BoosterState> boosters = new();
        

        private void ChangeBoosterState(string boosterName, bool state)
        {
            var booster = boosters.FirstOrDefault(b => b.name == boosterName);
            
            if (booster == null)
            {
                boosters.Add(new BoosterState
                {
                    name = boosterName,
                    state = state
                });
                return;
            }

            booster.state = state;
        }

        public bool GetBoosterState(string boosterName)
        {
            var booster = boosters.FirstOrDefault(b => b.name == boosterName);
            return booster?.state ?? false;
        }
        
        private void Start()
        {
            InitializeButtons();
        }

        private void InitializeButtons()
        {
            autoClickerButton.onActivateBooster += () => ChangeBoosterState("auto_clicker", true);
            autoClickerButton.onDeactivateBooster += () => ChangeBoosterState("auto_clicker", false);

            rewardBooster.onActivateBooster += () =>
            {
                ChangeBoosterState("reward_booster", true);
                StageController.Instance.CurrentStage.SetupCurrentRewards();
            };
            rewardBooster.onDeactivateBooster += () =>
            {
                ChangeBoosterState("reward_booster", false);
                StageController.Instance.CurrentStage.SetupCurrentRewards();
            };
        }
    }
}