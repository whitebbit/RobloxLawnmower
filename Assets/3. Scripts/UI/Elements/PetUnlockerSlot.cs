using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _3._Scripts.UI.Elements
{
    public class PetUnlockerSlot : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private TMP_Text percent;

        public void Initialize(Sprite image, float percentValue)
        {
            icon.sprite = image;
            percent.text = $"{percentValue}%";
        }
        
    }
}