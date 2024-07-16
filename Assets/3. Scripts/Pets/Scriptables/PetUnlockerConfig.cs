using System.Collections.Generic;
using UnityEngine;

namespace _3._Scripts.Pets.Scriptables
{
    [CreateAssetMenu(fileName = "PetUnlockerConfig", menuName = "ScriptableObjects/PetUnlockerConfig", order = 0)]
    public class PetUnlockerConfig : ScriptableObject
    {
        [SerializeField] private List<PetData> pets =new();
        [SerializeField] private float price;

        public List<PetData> Pets => pets;

        public float Price => price;
    }
}