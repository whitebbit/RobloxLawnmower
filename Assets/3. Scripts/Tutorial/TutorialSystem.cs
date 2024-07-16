using System;
using _3._Scripts.Singleton;
using UnityEngine;

namespace _3._Scripts.Tutorial
{
    public static class TutorialSystem 
    {
        public static event Action<string> TutorialStepStart;
        public static event Action<string> TutorialStepComplete;

        public static void StepComplete(string stepName)
        {
            TutorialStepComplete?.Invoke(stepName);
        }

        public  static void StepStart(string stepName)
        {
            TutorialStepStart?.Invoke(stepName);
        }
        
    }
}
