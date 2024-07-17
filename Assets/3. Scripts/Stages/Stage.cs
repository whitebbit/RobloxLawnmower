using System;
using System.Collections.Generic;
using System.Linq;
using _3._Scripts.Actions;
using _3._Scripts.Bots;
using _3._Scripts.Pets;
using _3._Scripts.Stages.Enums;
using _3._Scripts.Stages.Scriptable;
using UnityEngine;
using VInspector;

namespace _3._Scripts.Stages
{
    public class Stage : MonoBehaviour
    {
        [Header("Main")] [SerializeField] private StageConfig config;
        [SerializeField] private Transform spawnPoint;
        [SerializeField] private List<Bot> bots = new();
        private List<Grass> _grasses = new();

        private readonly List<Bot> _currentBots = new();
        public Transform SpawnPoint => spawnPoint;
        public float GiftBooster => config.GiftBooster;
        public int ID => config.ID;

        public void Initialize()
        {
            InitializePetUnlocker();
            InitializeBots();
            InitializeTeleport();
            InitializeGrasses();
        }

        private void InitializeTeleport()
        {
            var obj = GetComponentsInChildren<Teleport>().FirstOrDefault(s => s.Type == TeleportType.Next);
            if (obj != null)
                obj.SetPrice(config.TeleportPrice);
        }
        private void InitializeGrasses()
        {
            _grasses = new List<Grass>(GetComponentsInChildren<Grass>());
            foreach (var grass in _grasses)
            {
                grass.Respawn();
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