using _3._Scripts.UI.Interfaces;
using _3._Scripts.UI.Panels;
using DG.Tweening;
using GBGamesPlugin;
using UnityEngine;

namespace _3._Scripts.UI
{
    public abstract class UIPanel : UIElement
    {
        protected override void OnOpen()
        {
            base.OnOpen();
            if (!(this is MiniGamePanel))
                GBGames.GameplayStopped();
            UIManager.Instance.Active = true;
        }

        protected override void OnClose()
        {
            base.OnClose();
            if (!(this is MiniGamePanel))
                GBGames.GameplayStarted();
            UIManager.Instance.Active = false;
        }
    }
}