using System;
using System.Collections;
using System.Collections.Generic;
using _3._Scripts.Stages;
using _3._Scripts.UI.Elements;
using _3._Scripts.UI.Panels.Base;
using UnityEngine;

namespace _3._Scripts.UI.Panels
{
    public class FreeGiftsPanel : SimplePanel
    {
        [SerializeField] private List<GiftSlot> slots;
        public event Action ONOpen;

        public override void Initialize()
        {
            InTransition = transition;
            OutTransition = transition;
            foreach (var slot in slots)
            {
                slot.Initialize();
            }
        }

        protected override void OnOpen()
        {
            base.OnOpen();
            ONOpen?.Invoke();
            foreach (var slot in slots)
            {
                slot.Initialize();
            }
        }
    }
}