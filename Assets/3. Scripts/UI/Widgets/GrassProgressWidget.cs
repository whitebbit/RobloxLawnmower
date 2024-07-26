using System.Collections.Generic;
using _3._Scripts.Currency.Enums;
using _3._Scripts.Stages;
using _3._Scripts.UI.Effects;
using _3._Scripts.UI.Elements;
using _3._Scripts.UI.Interfaces;
using _3._Scripts.UI.Panels;
using _3._Scripts.UI.Transitions;
using DG.Tweening;
using GBGamesPlugin;
using UnityEngine;
using UnityEngine.UI;
using VInspector;

namespace _3._Scripts.UI.Widgets
{
    public class GrassProgressWidget : UIWidget
    {
        [Tab("Widget Settings")] [SerializeField]
        private FadeTransition transition;

        [Tab("Components")] [SerializeField] private Slider progressBar;
        [SerializeField] private List<ProgressButton> buttons = new();
        [Tab("Rewards")] [SerializeField] private CurrencyType rewardType;
        [SerializeField] private CurrencyCounterEffect effect;

        public override IUITransition InTransition { get; set; }
        public override IUITransition OutTransition { get; set; }

        public override void Initialize()
        {
            InTransition = transition;
            OutTransition = transition;
        }

        public void Setup(List<float> rewards)
        {
            StageController.Instance.CurrentStage.OnGrassShaved -= OnGrassShaved;
            
            for (var i = 0; i < buttons.Count; i++)
            {
                var i1 = i;
                buttons[i].RemoveAllListeners();
                buttons[i].Interactable = progressBar.value >= 0.33 * (i + 1);
                buttons[i].Initialize(rewards[i]);
                buttons[i].AddListener(() => GetReward(rewards[i1]));
            }

            StageController.Instance.CurrentStage.OnGrassShaved += OnGrassShaved;
        }

        private void OnGrassShaved(float percent)
        {
            progressBar.DOValue(percent, 0.1f);

            for (var i = 0; i < buttons.Count; i++)
            {
                buttons[i].Interactable = percent >= 0.3 * (i + 1);
            }
        }

        private void GetReward(float reward)
        {
            var effectInstance = CurrencyEffectPanel.Instance.SpawnEffect(effect, rewardType, reward);
            effectInstance.Initialize(rewardType, reward);
            
            GBGames.saves.achievementSaves.Update("cups_1", reward);
            GBGames.saves.achievementSaves.Update("cups_100", reward);
            GBGames.saves.achievementSaves.Update("cups_1000", reward);

            StageController.Instance.CurrentStage.RespawnGrassFields();
        }
    }
}