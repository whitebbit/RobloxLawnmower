using System;
using System.Collections.Generic;
using System.Linq;
using _3._Scripts.Singleton;
using _3._Scripts.Stages;
using _3._Scripts.UI;
using _3._Scripts.UI.Widgets;
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
                UIManager.Instance.GetWidget<GrassProgressWidget>().Setup();
            };
            rewardBooster.onDeactivateBooster += () =>
            {
                ChangeBoosterState("reward_booster", false);
                UIManager.Instance.GetWidget<GrassProgressWidget>().Setup();
            };
        }
    }
}