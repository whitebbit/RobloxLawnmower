using System;
using System.Collections;
using System.Collections.Generic;
using _3._Scripts.UI.Scriptable.Shop;
using _3._Scripts.Upgrades;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace _3._Scripts.Characters
{
    [ExecuteInEditMode]
    public class Character : MonoBehaviour
    {
        private Animator _animator;
        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
           
        }
#endif
        
        private void Start()
        {
            Player.Player.instance.PlayerAnimator.SetAvatar(_animator.avatar);
        }

        public void SetUpgrade(UpgradeItem upgrade)
        {
           
        }

    }
}