using System;
using System.Collections.Generic;
using System.Linq;
using _3._Scripts.Singleton;
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
        [SerializeField] private BoosterButton healthBooster;
        [SerializeField] private BoosterButton rewardBooster;
        [SerializeField] private BoosterButton slapBooster;
        [Tab("View")]
        [SerializeField] private Transform healthBoosterView;
        [SerializeField] private Transform slapBoosterView;
        
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
            //healthBoosterView.gameObject.SetActive(false);
            //slapBoosterView.gameObject.SetActive(false);
            
            InitializeButtons();
        }

        private void InitializeButtons()
        {
            autoClickerButton.onActivateBooster += () => ChangeBoosterState("auto_clicker", true);
            autoClickerButton.onDeactivateBooster += () => ChangeBoosterState("auto_clicker", false);

            healthBooster.onActivateBooster += () =>
            {
                healthBoosterView.gameObject.SetActive(true);
                ChangeBoosterState("health_booster", true);
            };
            healthBooster.onDeactivateBooster += () =>
            {
                healthBoosterView.gameObject.SetActive(false);
                ChangeBoosterState("health_booster", false);
            };

            rewardBooster.onActivateBooster += () => ChangeBoosterState("reward_booster", true);
            rewardBooster.onDeactivateBooster += () => ChangeBoosterState("reward_booster", false);

            slapBooster.onActivateBooster += () =>
            {
                slapBoosterView.gameObject.SetActive(true);
                ChangeBoosterState("slap_booster", true);
            };
            slapBooster.onDeactivateBooster += () =>
            {
                slapBoosterView.gameObject.SetActive(false);
                ChangeBoosterState("slap_booster", false);
            };
        }
    }
}