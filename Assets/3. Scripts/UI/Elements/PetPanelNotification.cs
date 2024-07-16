using System;
using System.Linq;
using _3._Scripts.Config;
using _3._Scripts.Wallet;
using GBGamesPlugin;
using UnityEngine;

namespace _3._Scripts.UI.Elements
{
    public class PetPanelNotification : MonoBehaviour
    {
        [SerializeField] private Transform notification;

        private void OnEnable()
        {
            GBGames.saves.petsSave.ONPetUnlocked += OnChange;
        }

        private void OnDisable()
        {
            GBGames.saves.petsSave.ONPetUnlocked -= OnChange;
        }

        private void OnChange()
        {
            notification.gameObject.SetActive(true);
        }
    }
}