using UnityEngine;

namespace _3._Scripts.Stages.Scriptable
{
    [CreateAssetMenu(fileName = "GrassData", menuName = "ScriptableObjects/GrassData", order = 1)]
    public class GrassData : ScriptableObject
    {
        [SerializeField] private Grass prefab;
        [SerializeField] private float resistance;

        public Grass Prefab => prefab;
        public float Resistance => resistance;
    }
}