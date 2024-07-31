using System.Collections.Generic;
using _3._Scripts.Currency.Enums;
using _3._Scripts.Stages;
using _3._Scripts.Tutorial;
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
        [SerializeField] private Transform tutorial;

       

        public override IUITransition InTransition { get; set; }
        public override IUITransition OutTransition { get; set; }

        public override void Initialize()
        {
            InTransition = transition;
            OutTransition = transition;
        }

        public void Setup()
        {
            StageController.Instance.CurrentStage.OnGrassShaved -= OnGrassShaved;
            var rewards = StageController.Instance.CurrentStage.CurrentRewards();
            var count = Mathf.Min(buttons.Count, rewards.Count);

            for (var i = 0; i < count; i++)
            {
                buttons[i].Interactable = progressBar.value >= 0.3f * (i + 1);
                buttons[i].Initialize(rewards[i]); 
            }

            StageController.Instance.CurrentStage.OnGrassShaved += OnGrassShaved;
            tutorial.gameObject.SetActive(false);
        }

        private void OnGrassShaved(float percent)
        {
            progressBar.DOValue(percent, 0.1f);

            for (var i = 0; i < buttons.Count; i++)
            {
                buttons[i].Interactable = percent >= 0.3 * (i + 1);
            }

            if (!GBGames.saves.tutorialComplete && buttons[0].Interactable)
            {
                tutorial.gameObject.SetActive(true);
            }
        }
        
    }
}