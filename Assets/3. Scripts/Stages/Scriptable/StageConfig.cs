using System.Collections.Generic;
using _3._Scripts.Actions.Scriptable;
using _3._Scripts.Enemies.Scriptable;
using _3._Scripts.Pets.Scriptables;
using UnityEngine;

namespace _3._Scripts.Stages.Scriptable
{
    [CreateAssetMenu(fileName = "StageConfig", menuName = "ScriptableObjects/StageConfig", order = 0)]
    public class StageConfig : ScriptableObject
    {
        [SerializeField, Min(0)] private int id;
        [SerializeField, Min(0)] private float teleportPrice;
        [SerializeField] private float giftBooster;
        [Header("Configs")] 
        [SerializeField] private PetUnlockerConfig petUnlocker;
        [SerializeField] private List<TrainingConfig> trainings = new ();

        public float GiftBooster => giftBooster;
        public PetUnlockerConfig PetUnlocker => petUnlocker;
        public List<TrainingConfig> Trainings => trainings;
        public int ID => id;

        public float TeleportPrice => teleportPrice;
        
        
    }
}