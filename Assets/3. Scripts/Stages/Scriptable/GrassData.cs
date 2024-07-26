using UnityEngine;

namespace _3._Scripts.Stages.Scriptable
{
    [CreateAssetMenu(fileName = "GrassData", menuName = "ScriptableObjects/GrassData", order = 1)]
    public class GrassData : ScriptableObject
    {
        [SerializeField] private Color grassColor;
        [SerializeField] private Color grassParticleColor;
        
        [SerializeField] private float resistance;

        public Color GrassParticleColor => grassParticleColor;
        public Color GrassColor => grassColor;
        public float Resistance => resistance;
    }
}