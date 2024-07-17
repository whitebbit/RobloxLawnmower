using _3._Scripts.Stages;
using _3._Scripts.UI.Panels.Base;
using UnityEngine;
using UnityEngine.UI;

namespace _3._Scripts.UI.Panels
{
    public class WorldUpdaterPanel : SimplePanel
    {
        [SerializeField] private Button confirm;

        public override void Initialize()
        {
            base.Initialize();
            confirm.onClick.AddListener(() =>
            {
                StageController.Instance.TeleportToNextWorld();
                Enabled = false;
            });
        }
    }
}