using System;
using System.Collections.Generic;
using System.Linq;
using _3._Scripts.Saves;
using _3._Scripts.Singleton;
using _3._Scripts.Wallet;
using GBGamesPlugin;
using UnityEngine;
using VInspector;

namespace _3._Scripts.Stages
{
    public class StageController : Singleton<StageController>
    {
        [SerializeField] private List<World> worlds = new();
     
        public Stage CurrentStage { get; private set; }
        public int CurrentStageID { get; private set; }

        private void Start()
        {
#if UNITY_EDITOR
            if (Application.isPlaying)
                TeleportToStage(GBGames.saves.stageID);
#else
             TeleportToStage(GBGames.saves.stageID);
#endif
        }

        public void TeleportToNextStage()
        {
            CurrentStageID += 1;
            if (CurrentStageID > GBGames.saves.stageID)
                GBGames.saves.stageID = CurrentStageID;
            TeleportToStage(CurrentStageID);
            GBGames.instance.Save();
        }

        public void TeleportToPreviousStage()
        {
            CurrentStageID -= 1;
            TeleportToStage(CurrentStageID);
        }

        public void TeleportToNextWorld()
        {
            GBGames.saves.worldID += 1;
            GBGames.saves.stageID = 0;
            
            Player.Player.instance.Reborn();
            TeleportToStage(0);
        }

        private void TeleportToStage(int stageID)
        {
            var world = worlds.FirstOrDefault(w => w.ID == GBGames.saves.worldID);

            if (world == null) return;

            var stage = world.Stages.FirstOrDefault(s => s.ID == stageID);

            if (stage == null) return;

            CurrentStage = stage;
            CurrentStageID = stageID;

            foreach (var s in worlds.SelectMany(w => w.Stages))
            {
                s.gameObject.SetActive(false);
            }

            var spawnPoint = stage.SpawnPoint.position;

            stage.gameObject.SetActive(true);
            stage.Initialize();

            Player.Player.instance.Teleport(spawnPoint);
        }
    }
}