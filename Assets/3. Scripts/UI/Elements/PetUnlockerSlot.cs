using _3._Scripts.Config;
using _3._Scripts.UI.Enums;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _3._Scripts.UI.Elements
{
    public class PetUnlockerSlot : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private TMP_Text percent;
        [SerializeField] private Image table;
        
        public void Initialize(Sprite image, Rarity rarity, float percentValue)
        {
            icon.sprite = image;
            percent.text = $"{percentValue}%";
            table.color = Configuration.Instance.GetRarityTable(rarity).MainColor;
            
        }
        
    }
}