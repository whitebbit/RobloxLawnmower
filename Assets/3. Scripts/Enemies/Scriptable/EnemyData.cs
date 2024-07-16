using UnityEngine;
using UnityEngine.Serialization;
using VInspector;

namespace _3._Scripts.Enemies.Scriptable
{
    [CreateAssetMenu(fileName = "Enemy Data", menuName = "ScriptableObjects/EnemyData", order = 0)]
    public class EnemyData : ScriptableObject
    {
        [Tab("UI")] 
        [SerializeField] private string localizationID;
        [SerializeField] private Sprite icon;
        [Tab("Settings")]
        [SerializeField] private float health;
        [SerializeField] private float strength;
        [SerializeField] private float rewardCount;

        [SerializeField] private ComplexityType complexityType;
        [Tab("View")] [SerializeField]
        private Transform model;

        public float RewardCount => rewardCount;
        public Transform Model => model;
        public string LocalizationID => localizationID;

        public float Health => health;
        public Sprite Icon => icon;

        public float Strength => strength;

        public ComplexityType ComplexityType => complexityType;
    }
}