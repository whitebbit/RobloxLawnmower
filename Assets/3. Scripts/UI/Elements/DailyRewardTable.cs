using _3._Scripts.DailyRewards;
using _3._Scripts.Localization;
using _3._Scripts.UI.Extensions;
using _3._Scripts.UI.Scriptable.Roulette;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace _3._Scripts.UI.Elements
{
    public class DailyRewardTable : MonoBehaviour
    {
        [SerializeField] private LocalizeStringEvent dayText;
        [SerializeField] private TMP_Text title;
        [SerializeField] private Image icon;
        [SerializeField] private Image gotImage;


        private GiftItem _giftItem;
        public void Initialize(GiftItem item, int day)
        {
            _giftItem = item;
            
            title.text = item.Title();
            icon.sprite = item.Icon();
            icon.ScaleImage();
            
            _dayIndex = day;
            dayText.SetVariable("value", day +1);
            UpdateClaimState();
        }

        private int _dayIndex;

        public void UpdateClaimState()
        {
            gotImage.gameObject.SetActive(DailyRewardsController.Instance.Claimed(_dayIndex));
        }

        public void UpdateReward()
        {
            title.text = _giftItem.Title();
            icon.sprite = _giftItem.Icon();
            icon.ScaleImage();
        }
    }
}