using UnityEngine;
using VInspector;

namespace _3._Scripts.Player.Scriptables
{
    [CreateAssetMenu(fileName = "LawnmowerData", menuName = "ScriptableObjects/LawnmowerData", order = 1)]
    public class 
        LawnmowerData : ScriptableObject
    {
        [SerializeField] private int level;
        
        [Tab("Properties")] 
        [SerializeField] private float radius;
        [SerializeField] private float boltsToUnlock;
        [SerializeField] private float cupsBooster;
        
        [Tab("View")]
        [SerializeField] private Color color;
        [SerializeField] private Transform effectPrefab;

        
        public float CupsBooster => cupsBooster;
        public int Level => level;
        public float BoltsToUnlock => boltsToUnlock;
        public Color Color => color;
        public float Radius => radius;
        public Transform EffectPrefab => effectPrefab;
    }   
}