using System.Collections.Generic;
using _3._Scripts.DailyRewards;
using _3._Scripts.UI.Elements;
using _3._Scripts.UI.Panels.Base;
using UnityEngine;
using UnityEngine.UI;

namespace _3._Scripts.UI.Panels
{
    public class DailyRewardPanel : SimplePanel
    {
        [SerializeField] private DailyRewardTable tablePrefab;
        [SerializeField] private Transform container;
        [SerializeField] private Button claim;
        
        private readonly List<DailyRewardTable> _tables = new();

        public override void Initialize()
        {
            base.Initialize();
            var day = 0;
            foreach (var giftItem in DailyRewardsController.Instance.Rewards)
            {
                var obj = Instantiate(tablePrefab, container);
                obj.Initialize(giftItem, day);
                _tables.Add(obj);
                day++;
            }
            claim.onClick.AddListener(() =>
            {
                DailyRewardsController.Instance.CollectRewards();
                foreach (var table in _tables)
                {
                    table.UpdateClaimState();
                }
            });
        }

        protected override void OnOpen()
        {
            base.OnOpen();
            DailyRewardsController.Instance.CheckForNew();
            foreach (var table in _tables)
            {
                table.UpdateReward();
            }
        }
    }
}