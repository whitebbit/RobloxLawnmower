using System;
using System.Collections.Generic;
using System.Linq;
using _3._Scripts.Actions;
using _3._Scripts.Boosters;
using _3._Scripts.Bots;
using _3._Scripts.Config;
using _3._Scripts.Pets;
using _3._Scripts.Stages.Enums;
using _3._Scripts.Stages.Scriptable;
using _3._Scripts.UI;
using _3._Scripts.UI.Widgets;
using GBGamesPlugin;
using UnityEngine;
using VInspector;

namespace _3._Scripts.Stages
{
    public class Stage : MonoBehaviour
    {
        [Header("Main")] [SerializeField] private StageConfig config;
        [SerializeField] private Transform spawnPoint;
        [SerializeField] private List<Bot> bots = new();

        private readonly List<Grass> _grasses = new();
        private List<GrassField> _grassFields = new();
        private readonly List<Bot> _currentBots = new();
        public Transform SpawnPoint => spawnPoint;
        public float GiftBooster => config.GiftBooster;
        public int ID => config.ID;
        public int MaxBoltCount => config.MaxBoltCount;
        public event Action<float> OnGrassShaved;

        public void Initialize()
        {
            InitializePetUnlocker();
            InitializeBots();
            InitializeTeleport();
            InitializeGrassFields();
            InitializeTraining();

            OnGrassShaved = null;
        }

        public bool IsMethodSubscribed(Action<float> method)
        {
            if (OnGrassShaved == null) return false;

            var invocationList = OnGrassShaved.GetInvocationList();
            return invocationList.Any(d => d.Method == method.Method);
        }

        public List<float> CurrentRewards()
        {
            var baseRewards = config.BaseRewardsCount;
            var currentLawnmower = Configuration.Instance.AllLawnmower
                .FirstOrDefault(l => l.Level == GBGames.saves.lawnmowerLevel);
            var lawnmowerCupsBooster = currentLawnmower == null ? 1 : currentLawnmower.CupsBooster;
            var booster = BoostersHandler.Instance.GetBoosterState("reward_booster") ? 2 : 1;
            var currentRewards = from baseReward in baseRewards select baseReward * lawnmowerCupsBooster * booster;
            return currentRewards.ToList();
        }

        public void OnGrassCutDown()
        {
            var shavedCount = _grasses.Count(g => g.Shaved);
            var allGrass = _grasses.Count;
            var percent = allGrass == 0 ? 0 : (float) shavedCount / allGrass;
            OnGrassShaved?.Invoke(percent);
        }

        public void RespawnGrassFields()
        {
            foreach (var grassField in _grassFields)
            {
                grassField.Respawn();
            }

            Player.Player.instance.SetMowingState(false);
            Player.Player.instance.Teleport(spawnPoint.position);
            OnGrassShaved?.Invoke(0);
        }

        private void InitializeTraining()
        {
            var obj = GetComponentsInChildren<Training>();
            var trainIndex = 0;
            foreach (var training in obj)
            {
                training.Initialize(config.Trainings[trainIndex]);
                trainIndex++;
                if (trainIndex >= config.Trainings.Count) break;
            }
        }

        private void InitializeTeleport()
        {
            var obj = GetComponentsInChildren<Teleport>().FirstOrDefault(s => s.Type == TeleportType.Next);
            if (obj != null)
                obj.SetPrice(config.TeleportPrice);
        }

        private void InitializeGrassFields()
        {
            _grasses.Clear();
            _grassFields = GetComponentsInChildren<GrassField>().ToList();
            var grassIndex = 0;
            foreach (var list in _grassFields.Select(grassField => grassField.Initialize(config.GrassData[grassIndex])))
            {
                _grasses.AddRange(list);
                grassIndex++;
                if (grassIndex >= config.GrassData.Count) break;
            }
        }

        private void InitializePetUnlocker()
        {
            var obj = GetComponentsInChildren<PetUnlocker>();
            var index = 0;
            foreach (var petUnlocker in obj)
            {
                petUnlocker.Initialize(config.PetUnlockers[index]);
                index++;
                if (index >= config.PetUnlockers.Count) break;
            }
        }

        private void OnValidate()
        {
            gameObject.name = $"Stage_{ID}";
        }

        private void InitializeBots()
        {
            foreach (var obj in bots.Select(bot => Instantiate(bot, transform)))
            {
                obj.transform.position += Vector3.left * UnityEngine.Random.Range(-7.5f, 7.5f) +
                                          Vector3.forward * UnityEngine.Random.Range(-7.5f, 7.5f);
                _currentBots.Add(obj);
            }
        }

        private void OnDisable()
        {
            foreach (var bot in _currentBots)
            {
                Destroy(bot.gameObject);
            }

            _currentBots.Clear();
        }
    }
}