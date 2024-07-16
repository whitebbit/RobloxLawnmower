using System;
using _3._Scripts.Wallet;
using UnityEngine;

namespace _3._Scripts.Tutorial
{
    public class TutorialController : MonoBehaviour
    {
        private void Start()
        {
            if (WalletManager.FirstCurrency >= 120) return;

            TutorialSystem.StepStart("training");
        }

        private void OnEnable()
        {
            if (WalletManager.FirstCurrency >= 120) return;
            
            WalletManager.OnFirstCurrencyChange += (_, f1) =>
            {
                if (!(f1 >= 120)) return;
                TutorialSystem.StepComplete("training");
                TutorialSystem.StepStart("fight");
            };
        }
    }
}