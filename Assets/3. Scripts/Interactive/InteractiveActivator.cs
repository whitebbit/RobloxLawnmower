using System;
using _3._Scripts.Ads;
using _3._Scripts.Interactive.Interfaces;
using _3._Scripts.UI;
using _3._Scripts.UI.Panels;
using GBGamesPlugin;
using UnityEngine;
using UnityEngine.UI;

namespace _3._Scripts.Interactive
{
    public abstract class InteractiveActivator<T> : MonoBehaviour where T : IInteractive
    {
        [SerializeField] private Image progressImage;
        [SerializeField] private float fillSpeed = 1;
        [SerializeField] private T interactive;

        private bool _waitToInteract;

        private void Start()
        {
            progressImage.fillAmount = 0;
        }

        private void Update()
        {
            ChangeProgressBar();
        }

        private void ChangeProgressBar()
        {
            var value = Time.deltaTime * fillSpeed;
            var fillAmount = _waitToInteract ? value : -value;

            progressImage.fillAmount += fillAmount;

            TryInteract();
        }

        private void TryInteract()
        {
            if (!(progressImage.fillAmount >= 1)) return;

            _waitToInteract = false;
            progressImage.fillAmount = 0;
            interactive.Interact();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent(out Player.Player _)) return;

            _waitToInteract = true;
        }

        private void OnTriggerStay(Collider other)
        {
            if (!other.TryGetComponent(out Player.Player _)) return;

            if (UIManager.Instance.Active || InterstitialsTimer.Instance.Active || GBGames.NowAdsShow) return;

            _waitToInteract = true;
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.TryGetComponent(out Player.Player _)) return;

            _waitToInteract = false;
        }

        protected virtual bool CanInteract()
        {
            return true;
        }
    }
}