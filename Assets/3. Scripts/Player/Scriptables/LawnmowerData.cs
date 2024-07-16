using UnityEngine;
using VInspector;

namespace _3._Scripts.Player.Scriptables
{
    [CreateAssetMenu(fileName = "LawnmowerData", menuName = "ScriptableObjects/LawnmowerData", order = 1)]
    public class LawnmowerData : ScriptableObject
    {
        [Tab("Properties")] 
        [SerializeField] private float radius;
        [Tab("View")]
        [SerializeField] private Color color;
        [SerializeField] private Transform effectPrefab;

        public Color Color => color;
        public float Radius => radius;
        public Transform EffectPrefab => effectPrefab;
    }   
}