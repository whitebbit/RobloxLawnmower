using System;
using _3._Scripts.MiniGame;
using _3._Scripts.Wallet;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace _3._Scripts.UI.Elements
{
    [Serializable]
    public class FighterTable
    {
        [SerializeField] private Image icon;
        [SerializeField] private Slider healthBar;
        [SerializeField] private TMP_Text healthText;

        private float _currentHealth;
        private FighterData _data;


        public void Initialize(FighterData data)
        {
            _data = data;
            icon.sprite = data.photo;
            healthBar.value = 1;
            healthText.text =
                $"{WalletManager.ConvertToWallet((decimal) _data.health)}/{WalletManager.ConvertToWallet((decimal) _data.health)}";
            _currentHealth = data.health;
        }

        public void TakeDamage(float damage, Action onDead = null)
        {
            _currentHealth -= damage;
            _currentHealth = Math.Clamp(_currentHealth, 0, _data.health);
            var value = _currentHealth / _data.health;

            healthText.text =
                $"{WalletManager.ConvertToWallet((decimal) _currentHealth)}/{WalletManager.ConvertToWallet((decimal) _data.health)}";
            healthBar.DOValue(value, 0.1f).OnComplete(() =>
            {
                if (_currentHealth <= 0)
                    onDead?.Invoke();
            });
        }
    }
}