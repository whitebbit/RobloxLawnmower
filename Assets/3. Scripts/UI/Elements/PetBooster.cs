using _3._Scripts.Config;
using _3._Scripts.Pets.Scriptables;
using _3._Scripts.Saves;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _3._Scripts.UI.Elements
{
    public class PetBooster: MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private TMP_Text counter;

        public void SetBooster(PetSaveData saveData)
        {
            var data = Configuration.Instance.GetPet(saveData.dataID);
            var currency = Configuration.Instance.GetCurrency(data.BoosterType);
            icon.sprite = currency.Icon;
            counter.text = $"+{saveData.booster}";
        }
        
    }
}