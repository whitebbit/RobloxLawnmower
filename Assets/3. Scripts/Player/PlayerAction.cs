using System;
using System.Linq;
using _3._Scripts.Actions.Interfaces;
using _3._Scripts.Ads;
using _3._Scripts.Boosters;
using _3._Scripts.Config;
using _3._Scripts.Detectors;
using _3._Scripts.Detectors.Interfaces;
using _3._Scripts.Inputs;
using _3._Scripts.Inputs.Interfaces;
using _3._Scripts.Interactive.Interfaces;
using _3._Scripts.Sounds;
using _3._Scripts.UI;
using _3._Scripts.UI.Scriptable.Shop;
using GBGamesPlugin;
using UnityEngine;
using VInspector;

namespace _3._Scripts.Player
{
    public class PlayerAction : MonoBehaviour
    {
        [SerializeField] private float baseAnimationTime = 1;
        [SerializeField] private BaseDetector<IActionable> detector;

        private IInput _input;
        private bool _isOnCooldown;
        private float _cooldownTimer;
        private PlayerAnimator _animator;

        private IActionable _actionable;

        private void Awake()
        {
            _animator = GetComponent<PlayerAnimator>();
            detector.OnFound += DetectorOnFound;
        }

        private void DetectorOnFound(IActionable obj)
        {
            _actionable = obj;
        }

        private void Start()
        {
            _input = InputHandler.Instance.Input;
            _animator.Event += AnimatorAction;
        }

        private void Update()
        {
            if ((_input.GetAction() || BoostersHandler.Instance.GetBoosterState("auto_clicker")) &&
                !UIManager.Instance.Active && !InterstitialsTimer.Instance.Active) DoAction();
        }

        private void DoAction()
        {
            if (_actionable == null) return;
            if (_isOnCooldown) return;

            _isOnCooldown = true;

            _animator.DoAction(baseAnimationTime);
        }

        private void AnimatorAction(string id)
        {
            switch (id)
            {
                case "Action":
                    SoundManager.Instance.PlayOneShot("action");
                    _actionable?.Action();
                    break;
                case "ActionEnd":
                    _isOnCooldown = false;
                    break;
            }
        }
    }
}