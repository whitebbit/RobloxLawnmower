using System;
using _3._Scripts.Wallet;
using GBGamesPlugin;
using UnityEngine;

namespace _3._Scripts.Tutorial
{
    public class TutorialController : MonoBehaviour
    {
        private void Start()
        {
            if (GBGames.saves.tutorialComplete) return;

            TutorialSystem.StepStart("training");
        }
    }
}