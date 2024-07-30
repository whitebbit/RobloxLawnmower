using System;
using System.Collections.Generic;
using _3._Scripts.UI.Panels.Base;
using DG.Tweening;
using GBGamesPlugin;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace _3._Scripts.UI.Panels
{
    public class TrainingPanel : SimplePanel
    {
        [SerializeField] private Button cancelButton;
        [SerializeField] private List<Transform> objectsToDeactivate = new();
        [SerializeField] private Transform tutorialText;
        [SerializeField] private Transform startTutorial;


        private Tween _currentTween;
        public void AddAction(UnityAction action) => cancelButton.onClick.AddListener(action);

        protected override void OnOpen()
        {
            base.OnOpen();
            startTutorial.gameObject.SetActive(!GBGames.saves.tutorialComplete);

            foreach (var obj in objectsToDeactivate)
            {
                obj.gameObject.SetActive(false);
            }

            _currentTween = tutorialText.DOScale(1.2f, 0.5f)
                .SetLoops(-1, LoopType.Yoyo) // Зацикливаем анимацию
                .SetEase(Ease.InOutQuad);
        }

        protected override void OnClose()
        {
            base.OnClose();
            
            startTutorial.gameObject.SetActive(false);

            foreach (var obj in objectsToDeactivate)
            {
                obj.gameObject.SetActive(true);
            }

            _currentTween.Kill();
            _currentTween = null;
            tutorialText.localScale = Vector3.one;
            cancelButton.onClick.RemoveAllListeners();
        }
    }
}