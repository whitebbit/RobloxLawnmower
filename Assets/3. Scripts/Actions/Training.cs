﻿using System;
using _3._Scripts.Actions.Scriptable;
using _3._Scripts.Currency.Enums;
using _3._Scripts.Inputs;
using _3._Scripts.Interactive.Interfaces;
using _3._Scripts.Localization;
using _3._Scripts.UI;
using _3._Scripts.UI.Effects;
using _3._Scripts.UI.Panels;
using _3._Scripts.Wallet;
using Cinemachine;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Localization.Components;
using VInspector;

namespace _3._Scripts.Actions
{
    public class Training : MonoBehaviour, IInteractive
    {
        [Tab("Components")] [SerializeField] private Transform tutorial;
        [SerializeField] private CinemachineVirtualCamera virtualCamera;
        [SerializeField] private Transform playerPoint;
        

        [Tab("Settings")] [SerializeField] private CurrencyType currencyType;
        [SerializeField] private CurrencyCounterEffect effect;
        [Tab("Texts")] [SerializeField] private LocalizeStringEvent countText;
        [SerializeField] private LocalizeStringEvent requiredText;

        private float _requiredCount;
        private float _count;
        private bool _canTraining;

        private void Start()
        {
            tutorial.gameObject.SetActive(false);
        }

        public void Initialize(TrainingConfig config)
        {
            _count = config.Count;
            _requiredCount = config.RequiredCount;

            requiredText.SetVariable("value", WalletManager.ConvertToWallet((decimal) _requiredCount));
            countText.SetVariable("value", WalletManager.ConvertToWallet((decimal) _count));
        }

        private void Update()
        {
            if (!_canTraining) return;

            if (Input.GetMouseButtonDown(0))
                Action();
        }

        private void Action()
        {
            var training = Player.Player.instance.GetTrainingStrength(_count);
            var obj = CurrencyEffectPanel.Instance.SpawnEffect(effect, currencyType, training);

            obj.Initialize(currencyType, training);
        }

        public void StartInteract()
        {
            if (WalletManager.GetQuantityByType(currencyType) < _requiredCount) return;
            tutorial.gameObject.SetActive(true);
        }

        public void Interact()
        {
            if (WalletManager.GetQuantityByType(currencyType) < _requiredCount) return;

            var panel = UIManager.Instance.GetPanel<TrainingPanel>();
            var player = Player.Player.instance;
           
            InputHandler.Instance.SetState(false);
            CameraController.Instance.SwapTo(virtualCamera);
            
            panel.Enabled = true;
            panel.AddAction(StopInteract);
            
            player.Movement.Blocked = true;
            player.Teleport(playerPoint.position);
            player.transform.forward = transform.forward;
            player.PlayerAnimator.SetTrigger("StartTraining");
            player.PlayerAnimator.SetBool("Training", true);
            
            _canTraining = true;
            tutorial.gameObject.SetActive(false);
        }

        public void StopInteract()
        {
            if (_canTraining)
            {
                var player = Player.Player.instance;
                
                player.PlayerAnimator.SetBool("Training", false);
                player.Movement.Blocked = false;
                
                InputHandler.Instance.SetState(true);
                CameraController.Instance.SwapToMain();
                UIManager.Instance.GetPanel<TrainingPanel>().Enabled = false;
                
                _canTraining = false;
            }
            
            tutorial.gameObject.SetActive(false);
        }
    }
}