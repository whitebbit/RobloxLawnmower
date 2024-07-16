using _3._Scripts.UI.Interfaces;
using _3._Scripts.UI.Panels;
using DG.Tweening;
using GBGamesPlugin;
using UnityEngine;

namespace _3._Scripts.UI
{
    public abstract class UIElement : MonoBehaviour, IUIElement
    {
        private bool _enabled;
        private bool onTransition;
        public abstract IUITransition InTransition { get; set; }
        public abstract IUITransition OutTransition { get; set; }

        public bool Enabled
        {
            get => _enabled;
            set
            {
                if (value)
                {
                    if (!(this is MiniGamePanel))
                        GBGames.GameplayStopped();
                    UIManager.Instance.Active = true;
                    Open();
                }
                else
                {
                    if (!(this is MiniGamePanel))
                        GBGames.GameplayStarted();
                    UIManager.Instance.Active = false;
                    Close();
                }

                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }

        private Tween currentTween;

        public abstract void Initialize();

        public virtual void ForceOpen()
        {
            gameObject.SetActive(true);
            InTransition.ForceIn();
            _enabled = true;
            OnOpen();
        }

        public virtual void ForceClose()
        {
            OnClose();
            OutTransition.ForceOut();
            _enabled = false;
            gameObject.SetActive(false);
        }

        private void Open()
        {
            if (onTransition)
            {
                currentTween?.Pause();
                currentTween?.Kill();
                currentTween = null;
            }

            onTransition = true;
            gameObject.SetActive(true);
            currentTween = InTransition.AnimateIn().OnComplete(() =>
            {
                _enabled = true;
                onTransition = false;
            });
            OnOpen();
        }

        private void Close()
        {
            if (onTransition)
            {
                currentTween?.Pause();
                currentTween?.Kill();
                currentTween = null;
            }

            onTransition = true;
            currentTween = OutTransition.AnimateOut().OnComplete(() =>
            {
                OnClose();
                _enabled = false;
                onTransition = false;
                gameObject.SetActive(false);
            });
        }

        protected virtual void OnOpen()
        {
        }

        protected virtual void OnClose()
        {
        }
    }
}