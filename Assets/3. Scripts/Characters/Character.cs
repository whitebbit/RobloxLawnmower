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
        [SerializeField]private Animator animator;

#if UNITY_EDITOR
        private void OnValidate()
        {
            animator = GetComponent<Animator>();
        }
        
#endif

        public void Initialize()
        {
            Player.Player.instance.PlayerAnimator.SetAvatar(animator.avatar);
        }
    }
}