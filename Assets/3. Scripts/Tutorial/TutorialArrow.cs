using System;
using _3._Scripts.Currency.Enums;
using _3._Scripts.Interactive;
using _3._Scripts.Wallet;
using GBGamesPlugin;
using UnityEngine;

namespace _3._Scripts.Tutorial
{
    public class TutorialArrow : MonoBehaviour
    {
        [SerializeField] private string stepName;
        [SerializeField] private Transform target;
        [SerializeField] private Transform model;

        private bool _active;

        private void Start()
        {
            model.gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            TutorialSystem.TutorialStepStart += OnTutorialStepStart;
            TutorialSystem.TutorialStepComplete += OnTutorialStepComplete;
        }

        
        private void OnDisable()
        {
            TutorialSystem.TutorialStepStart -= OnTutorialStepStart;
            TutorialSystem.TutorialStepComplete -= OnTutorialStepComplete;
        }
        
        private void OnTutorialStepStart(string obj)
        {
            if(obj != stepName) return;
            
            model.gameObject.SetActive(true);
            _active = true;
        }
        
        private void OnTutorialStepComplete(string obj)
        {
            if(obj != stepName) return;
            
            model.gameObject.SetActive(false);
            _active = false;
        }


        private void Update()
        {
            if(_active)
                transform.LookAt(target.transform.position);
        }
    }
}