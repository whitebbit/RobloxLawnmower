using System;
using System.Collections.Generic;

namespace _3._Scripts.Saves
{
    [Serializable]
    public class DailyRewardSave
    {
        public int currentStreak;
        public DateTime lastLoginDate = DateTime.MinValue;
        public List<int> collectedRewards = new();


        public void CollectReward(int id)
        {
            collectedRewards.Add(id);
        }
    }
}