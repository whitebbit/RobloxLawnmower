using _3._Scripts.Currency.Enums;
using _3._Scripts.UI.Enums;
using UnityEngine;
using VInspector;

namespace _3._Scripts.Pets.Scriptables
{
    [CreateAssetMenu(fileName = "PetData", menuName = "ScriptableObjects/PetData", order = 0)]
    public class PetData : ScriptableObject
    {
        [SerializeField] private string id;
        [Tab("Main")]
        [SerializeField] private float dropPercent;
        [SerializeField] private Rarity rarity;
        [SerializeField] private Pet prefab;
        [Tab("Booster")] 
        [SerializeField] private CurrencyType boosterType;
        [Space]
        [SerializeField] private float minBooster;
        [SerializeField] private float maxBooster;
        [Tab("UI")] 
        [SerializeField] private Sprite icon;

        public Sprite Icon => icon;
        public Rarity Rarity => rarity;
        public Pet Prefab => prefab;
        public CurrencyType BoosterType => boosterType;
        public int RandomBooster => Random.Range(minBooster, maxBooster);
        public float DropPercent => dropPercent;
        public string ID => id;
        
    }
}