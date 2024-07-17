using System;
using System.Collections.Generic;
using _3._Scripts.Actions;
using _3._Scripts.Ads;
using _3._Scripts.Boosters;
using _3._Scripts.FSM.Base;
using _3._Scripts.Inputs;
using _3._Scripts.Player;
using _3._Scripts.Sounds;
using _3._Scripts.UI;
using UnityEngine;
using Random = System.Random;

namespace _3._Scripts.Bots
{
    [Serializable]
    public class TrainingState: State
    {
        [SerializeField] private PlayerAnimator animator;

        private Training[] _trainings;
        private UnitNavMeshAgent _navMesh;
        private bool _isOnCooldown;
        private bool _canTrain;
        public void SetNavMeshAgent(UnitNavMeshAgent navMesh)
        {
            _navMesh = navMesh;
        }

        public void SetTrainings(Training[] trainings) => _trainings = trainings;
        
        public override void OnEnter()
        {
            base.OnEnter();
            var rand = UnityEngine.Random.Range(0, _trainings.Length);
            _isOnCooldown = false;
            animator.Event += AnimatorAction;
            _navMesh.StartMoving(_trainings[rand].transform.position);
        }

        public override void Update()
        {
            base.Update();

            if (_navMesh.OnPoint() && !_canTrain)
            {
                _canTrain = true;
                _navMesh.StopMoving();
            }

            if (_canTrain)
            {
                DoAction();
            }
        }

        private void DoAction()
        {
            if (_isOnCooldown) return;
            
            _isOnCooldown = true;
            animator.DoAction(3);
        }

        private void AnimatorAction(string id)
        {
            _isOnCooldown = id switch
            {
                "ActionEnd" => false,
                _ => _isOnCooldown
            };
        }

        public override void OnExit()
        {
            base.OnExit();
            _navMesh.StopMoving();
            _canTrain = false;
            _isOnCooldown = false;
            animator.Event -= AnimatorAction;
        }
    }
}