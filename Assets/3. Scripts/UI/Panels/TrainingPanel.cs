using System;
using _3._Scripts.UI.Panels.Base;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace _3._Scripts.UI.Panels
{
    public class TrainingPanel : SimplePanel
    {
        [SerializeField] private Button cancelButton;

        public void AddAction(UnityAction action) => cancelButton.onClick.AddListener(action);
        protected override void OnClose()
        {
            base.OnClose();
            cancelButton.onClick.RemoveAllListeners();
        }
    }
}