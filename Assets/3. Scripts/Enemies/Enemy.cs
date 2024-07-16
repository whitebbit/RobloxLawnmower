using System;
using System.Collections;
using _3._Scripts.Enemies.Scriptable;
using _3._Scripts.Localization;
using _3._Scripts.MiniGame;
using _3._Scripts.MiniGame.Interfaces;
using _3._Scripts.Player;
using _3._Scripts.Wallet;
using UnityEngine;
using UnityEngine.Localization.Components;
using VInspector;

namespace _3._Scripts.Enemies
{
    public class Enemy : Fighter
    {
        [Tab("Components")] 
        [SerializeField, Min(1)] private float attackSpeed;
        [SerializeField] private PlayerAnimator animator;
        [Tab("Texts")] [SerializeField]
        private Transform allTexts;
        [Space]
        [SerializeField] private LocalizeStringEvent nameText;
        [SerializeField] private LocalizeStringEvent complexityText;
        [SerializeField] private LocalizeStringEvent recommendationText;
        
        private FighterData _fighterData;
       

        public void Initialize(EnemyData data)
        {
            var obj = Instantiate(data.Model, transform);
            var anim = obj.GetComponent<Animator>();
            
            obj.localScale = Vector3.one * 0.5f;
            obj.localPosition = Vector3.zero;

            anim.enabled = false;
            
            _fighterData = new FighterData
            {
                health = data.Health,
                strength = data.Strength,
                photo = data.Icon
            };

            InitializeText(data);
            
            animator.SetAvatar(anim.avatar);
            animator.SetSpeed(0);
            animator.SetGrounded(true);
        }
        
        public override void StartFight()
        {
            base.StartFight();
            StartCoroutine(SlapCoroutine());
        }

        public override void EndFight(bool win)
        {
            base.EndFight(win);
            StopAllCoroutines();
        }

        public override void OnStart()
        {
            allTexts.gameObject.SetActive(false);
        }

        public override void OnEnd()
        {
            allTexts.gameObject.SetActive(true);
        }

        public override FighterData FighterData()
        {
            return _fighterData;
        }

        protected override PlayerAnimator Animator()
        {
            return animator;
        }

        private IEnumerator SlapCoroutine()
        {
            while (isFight)
            {
                Slap();
                yield return new WaitForSeconds(attackSpeed);
            }
        }

        private void InitializeText(EnemyData data)
        {
            nameText.SetReference(data.LocalizationID);
            complexityText.TextToComplexity(data.ComplexityType);
            recommendationText.SetVariable("value", WalletManager.ConvertToWallet((decimal) (data.Health / 25)));
        }
    }
}