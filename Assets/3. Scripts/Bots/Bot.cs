using System;
using System.Collections;
using System.Linq;
using _3._Scripts.Actions;
using _3._Scripts.Config;
using _3._Scripts.FSM.Base;
using _3._Scripts.Localization;
using _3._Scripts.Player;
using _3._Scripts.Stages;
using _3._Scripts.Upgrades;
using UnityEditor;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.XR;
using VInspector;
using Random = UnityEngine.Random;

namespace _3._Scripts.Bots
{
    public class Bot : MonoBehaviour
    {
        [SerializeField] private UnitNavMeshAgent navMesh;
        [SerializeField] private LocalizeStringEvent levelText;
        [SerializeField] private Transform lawnmower;

        [Tab("States")] [SerializeField] private RunState runState;

        private FSMHandler _fsmHandler;
        private IdleState _idleState;
        private PlayerAnimator _animator;
        private bool _running;
#if UNITY_EDITOR
        private void OnValidate()
        {
            FindAndAddHandComponent(transform);
        }

        private void FindAndAddHandComponent(IEnumerable parent)
        {
            foreach (Transform child in parent)
            {
                switch (child.name)
                {
                    case "mixamorig:RightHand":
                    {
                        var components = child.GetComponents<Component>();

                        foreach (var component in components)
                        {
                            if (!(component is Transform)) // Не удаляем Transform
                            {
                                Destroy(component);
                            }
                        }

                        break;
                    }
                    case "mixamorig:LeftHand":
                    {
                        var components = child.GetComponents<Component>();

                        foreach (var component in components)
                        {
                            if (!(component is Transform)) // Не удаляем Transform
                            {
                                Destroy(component);
                            }
                        }
                        
                        break;
                    }
                }

                FindAndAddHandComponent(child);
            }
        }
#endif

        private void Awake()
        {
            _animator = GetComponent<PlayerAnimator>();

            _fsmHandler = new FSMHandler();
            _idleState = new IdleState();

            _idleState.SetNavMeshAgent(navMesh);
            runState.SetNavMeshAgent(navMesh);

            _fsmHandler.AddTransition(_idleState, new FuncPredicate(() => !_running));
            _fsmHandler.AddTransition(runState, new FuncPredicate(() => _running));

            _fsmHandler.StateMachine.SetState(_idleState);
        }

        private void Start()
        {
            levelText.SetVariable("value", Random.Range(100, 500).ToString());
            SetMowingState(false);
            StartCoroutine(ChangeState());
        }

        private void Update()
        {
            _fsmHandler.StateMachine.Update();
        }

        public void SetMowingState(bool state)
        {
            lawnmower.gameObject.SetActive(state);
            _animator.SetMowingState(state);
        }

        private IEnumerator ChangeState()
        {
            var time = 0;
            while (true)
            {
                var rand = Random.Range(0, 2);
                switch (rand)
                {
                    case 0:
                        _running = false;
                        time = Random.Range(2, 3);
                        break;

                    case 1:
                        _running = true;
                        time = Random.Range(5, 10);
                        break;
                }

                yield return new WaitForSeconds(time);
            }
        }
    }
}