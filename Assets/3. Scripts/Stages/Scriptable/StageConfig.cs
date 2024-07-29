using System.Collections.Generic;
using _3._Scripts.Actions.Scriptable;
using _3._Scripts.Pets.Scriptables;
using UnityEngine;

namespace _3._Scripts.Stages.Scriptable
{
    [CreateAssetMenu(fileName = "StageConfig", menuName = "ScriptableObjects/StageConfig", order = 0)]
    public class StageConfig : ScriptableObject
    {
        [SerializeField, Min(0)] private int id;
        [SerializeField] private int maxBoltCount;
        [SerializeField] private float giftBooster;
        [SerializeField, Min(0)] private float teleportPrice;
        [Header("Configs")] 
        [SerializeField] private List<float> baseRewardsCount;
        [SerializeField] private List<PetUnlockerConfig> petUnlockers = new();
        [SerializeField] private List<TrainingConfig> trainings = new();
        [SerializeField] private List<GrassData> grassData = new();

        public int MaxBoltCount => maxBoltCount;
        public List<float> BaseRewardsCount => baseRewardsCount;
        public List<GrassData> GrassData => grassData;
        public List<TrainingConfig> Trainings => trainings;
        public float GiftBooster => giftBooster;
        public List<PetUnlockerConfig> PetUnlockers => petUnlockers;
        public int ID => id;

        public float TeleportPrice => teleportPrice;
        
        
    }
}