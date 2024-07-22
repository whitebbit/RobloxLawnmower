using System;
using System.Collections.Generic;
using System.Linq;
using _3._Scripts.Actions;
using _3._Scripts.Bots;
using _3._Scripts.Pets;
using _3._Scripts.Stages.Enums;
using _3._Scripts.Stages.Scriptable;
using _3._Scripts.UI;
using _3._Scripts.UI.Widgets;
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
        public List<float> BaseRewardsCount => config.BaseRewardsCount;
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
            var obj = GetComponentInChildren<PetUnlocker>();
            obj.Initialize(config.PetUnlocker);
        }

        private void OnValidate()
        {
            gameObject.name = $"Stage_{ID}";
        }

        private void InitializeBots()
        {
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