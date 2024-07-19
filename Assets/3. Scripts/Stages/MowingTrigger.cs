﻿using System;
using _3._Scripts.Stages.Enums;
using _3._Scripts.UI;
using _3._Scripts.UI.Widgets;
using UnityEngine;

namespace _3._Scripts.Stages
{
    public class MowingTrigger : MonoBehaviour
    {
        [SerializeField] private MowingTriggerType type;


        private void OnTriggerEnter(Collider other)
        {            
            if(!other.TryGetComponent(out Player.Player player)) return;

            var state = type switch
            {
                MowingTriggerType.Start => true,
                MowingTriggerType.End => false,
                _ => throw new ArgumentOutOfRangeException()
            };

            player.SetMowingState(state);
        }
    }
}