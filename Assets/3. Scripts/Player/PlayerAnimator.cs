using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _3._Scripts.Player
{
    public class PlayerAnimator : MonoBehaviour
    {
        private Animator _animator;
        private static readonly int Speed = Animator.StringToHash("Speed");
        private static readonly int Action = Animator.StringToHash("Action");
        private static readonly int Jump = Animator.StringToHash("Jump");
        private static readonly int IsGrounded = Animator.StringToHash("IsGrounded");
        private static readonly int ActionSpeed = Animator.StringToHash("ActionSpeed");
        private static readonly int SlapSideID = Animator.StringToHash("SlapSideID");


        public event Action<string> Event;
        private int _actionCount;
        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        public void SetState(bool state) => _animator.enabled = state;

        public void SetAvatar(Avatar avatar)
        {
            SetState(false);
            _animator.avatar = avatar;
            SetState(true);
        }

        public void SetBool(string id, bool state)
        {
            if (_animator == null) return;

            _animator.SetBool(Animator.StringToHash(id), state);

        }
        
        public void SetTrigger(string id)
        {
            if (_animator == null) return;

            _animator.SetTrigger(Animator.StringToHash(id));
        }
        
        public void SetSpeed(float speed)
        {
            if (_animator == null) return;
            _animator.SetFloat(Speed, speed);
        }

        public void DoAction(float actionSpeed = 1)
        {
            if (_animator == null) return;

            var actionID = _actionCount == 2 ? 0 : 1;
            
            _animator.SetInteger(SlapSideID, actionID);
            _animator.SetFloat(ActionSpeed, actionSpeed);
            _animator.SetTrigger(Action);
            
            _actionCount += 1;
            if (_actionCount > 2)
                _actionCount = 0;
        }

        public void DoJump()
        {
            if (_animator == null) return;

            _animator.SetTrigger(Jump);
            SetGrounded(false);
        }

        public void SetGrounded(bool grounded)
        {
            if (_animator == null) return;

            _animator.SetBool(IsGrounded, grounded);
        }

        public void AnimationEvent(string id) => Event?.Invoke(id);
    }
}