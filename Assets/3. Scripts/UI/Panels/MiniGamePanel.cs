using System;
using System.Collections;
using System.Collections.Generic;
using _3._Scripts.Ads;
using _3._Scripts.Boosters;
using UnityEngine;
using _3._Scripts.Currency.Enums;
using _3._Scripts.Inputs;
using _3._Scripts.MiniGame;
using _3._Scripts.UI.Effects;
using _3._Scripts.UI.Elements;
using _3._Scripts.UI.Extensions;
using _3._Scripts.UI.Panels.Base;
using VInspector;

namespace _3._Scripts.UI.Panels
{
    public class MiniGamePanel : SimplePanel
    {
        [Tab("Fighters")] [SerializeField] private FighterTable playerFighterTable;
        [SerializeField] private FighterTable enemyFighterTable;

        [Tab("Components")] [SerializeField] private CountdownTimer countdownTimer;
        [SerializeField] private List<Transform> deactivateComponents = new();

        [Tab("Reward")] [SerializeField] private CurrencyType rewardType;
        [SerializeField] private CurrencyCounterEffect effect;

        private Fighter _player;
        private Fighter _enemy;
        private Action _onGameEnd;
        private bool _playerWin;
        private float _rewardCount;

        public void StartMiniGame(Fighter player, Fighter enemy, float reward, Action onGameEnd = null)
        {
            InitializeFighters(player, enemy);
            _rewardCount = reward;
            _onGameEnd = onGameEnd;

            SetComponentsState(false);
            InputHandler.Instance.SetState(false);
            InterstitialsTimer.Instance.Blocked = true;

            countdownTimer.StartCountdown(StartFight);
        }

        private void InitializeFighters(Fighter player, Fighter enemy)
        {
            _player = player;
            _player.OnSlap += HandlePlayerSlap;

            playerFighterTable.Initialize(_player.FighterData());

            _enemy = enemy;
            _enemy.OnSlap += HandleEnemySlap;

            enemyFighterTable.Initialize(_enemy.FighterData());
        }

        private void HandlePlayerSlap()
        {
            _enemy.GetHit();
            enemyFighterTable.TakeDamage(_player.FighterData().strength, () =>
            {
                _playerWin = true;
                EndMiniGame(_player, _enemy);
            });
        }

        private void HandleEnemySlap()
        {
            _player.GetHit();
            playerFighterTable.TakeDamage(_enemy.FighterData().strength, () => EndMiniGame(_enemy, _player));
        }

        private void StartFight()
        {
            _enemy.StartFight();
            _player.StartFight();
        }

        private void EndMiniGame(Fighter winner, Fighter loser)
        {
            winner.EndFight(true);
            loser.EndFight(false);
            StartCoroutine(DelayEnd());
        }

        private IEnumerator DelayEnd()
        {
            yield return new WaitForSeconds(1.95f);
            _onGameEnd?.Invoke();
            SetComponentsState(true);
            InputHandler.Instance.SetState(true);
            InterstitialsTimer.Instance.Blocked = false;

            if (_playerWin)
            {
                HandlePlayerWin();
            }
        }

        private void HandlePlayerWin()
        {
            var rewardC = BoostersHandler.Instance.GetBoosterState("reward_booster") ? _rewardCount * 2 : _rewardCount;
            var effectInstance = CurrencyEffectPanel.Instance.SpawnEffect(effect, rewardType, rewardC);
            effectInstance.Initialize(rewardType, rewardC);
            _playerWin = false;
        }

        private void SetComponentsState(bool state)
        {
            foreach (var component in deactivateComponents)
            {
                component.gameObject.SetActive(state);
            }
        }
    }
}