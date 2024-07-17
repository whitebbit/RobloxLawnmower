using System;
using System.Collections.Generic;
using _3._Scripts.Achievements;
using _3._Scripts.Saves;
using UnityEngine.Serialization;

namespace GBGamesPlugin
{
    [Serializable]
    public class SavesGB
    {
        // Технические сохранения.(Не удалять)
        public int saveID;

        // Ваши сохранения, если вы привыкли пользоваться сохранением через объекты. Можно задать полям значения по умолчанию     
        public bool defaultLoaded;
        
        public SaveHandler<string> characterSaves = new();
        public SaveHandler<string> trailSaves = new();
        public SaveHandler<string> upgradeSaves = new();
        public WalletSave walletSave = new();
        public PetSave petsSave = new();
        public DailyRewardSave dailyReward = new();
        public AchievementSaves achievementSaves = new();
        
        public int stageID;
        public int worldID;
        public bool sound = true;
    }
}