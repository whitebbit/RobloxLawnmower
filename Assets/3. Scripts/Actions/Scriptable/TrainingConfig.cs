using UnityEngine;

namespace _3._Scripts.Actions.Scriptable
{
    [CreateAssetMenu(fileName = "TrainingConfig", menuName = "ScriptableObjects/TrainingConfig", order = 0)]
    public class TrainingConfig : ScriptableObject
    {
        [SerializeField] private float count;
        [SerializeField] private float requiredCount;
        [SerializeField] private Color color;
        

        public float Count => count;

        public Color Color => color;
        public float RequiredCount => requiredCount;
    }
}