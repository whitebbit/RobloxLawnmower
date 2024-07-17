using System;
using System.Collections.Generic;
using _3._Scripts.Singleton;
using _3._Scripts.UI.Scriptable.Roulette;
using GBGamesPlugin;
using UnityEngine;

namespace _3._Scripts.DailyRewards
{
    public class DailyRewardsController : Singleton<DailyRewardsController>
    {
        [SerializeField] private List<GiftItem> rewards = new();

        public IEnumerable<GiftItem> Rewards => rewards;

        private void Start()
        {
            CheckForNew();
        }

        public void CheckForNew()
        {
#if UNITY_EDITOR
            CheckForNewMinute();
#else
            CheckForNewDay();
#endif
        }

        private void CheckForNewDay()
        {
            var currentDate = DateTime.Now;
            if (GBGames.saves.dailyReward.lastLoginDate.Date >= currentDate) return;
            if ((currentDate - GBGames.saves.dailyReward.lastLoginDate).Days > 1)
            {
                ResetRewards();
            }
            else
            {
                GBGames.saves.dailyReward.currentStreak++;
            }

            GBGames.saves.dailyReward.lastLoginDate = currentDate;
        }

        private void CheckForNewMinute()
        {
            var currentDate = DateTime.Now;
            if (GBGames.saves.dailyReward.lastLoginDate.Date >= currentDate) return;

            if ((currentDate - GBGames.saves.dailyReward.lastLoginDate).TotalMinutes >= 1)
            {
                ResetRewards();
            }
            else
            {
                GBGames.saves.dailyReward.currentStreak++;
            }

            GBGames.saves.dailyReward.lastLoginDate = currentDate;
        }

        public void CollectRewards()
        {
            for (var i = 0; i <= GBGames.saves.dailyReward.currentStreak; i++)
            {
                if (GBGames.saves.dailyReward.collectedRewards.Contains(i)) continue;

                GiveReward(i);
                GBGames.saves.dailyReward.CollectReward(i);
            }

            if (rewards.Count <= GBGames.saves.dailyReward.currentStreak)
            {
                ResetRewards();
            }
        }

        private void GiveReward(int day)
        {
            var reward = rewards[day];
            reward.OnReward();
        }

        private void ResetRewards()
        {
            GBGames.saves.dailyReward.currentStreak = 0;
            GBGames.saves.dailyReward.collectedRewards.Clear();
        }


        public bool Claimed(int dayIndex)
        {
            return GBGames.saves.dailyReward.collectedRewards.Contains(dayIndex);
        }
    }
}