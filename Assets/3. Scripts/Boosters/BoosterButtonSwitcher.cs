using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using VInspector;

namespace _3._Scripts.Boosters
{
    public class BoosterButtonSwitcher : MonoBehaviour
    {
        [Tab("View")] 
        [SerializeField] private Sprite enableSprite;
        [SerializeField] private Sprite disableSprite;

        public Action onActivateBooster;
        public Action onDeactivateBooster;
        private Button _button;
        private bool _state;
        private void Awake()
        {
            _button = GetComponent<Button>();
        }

        private void Start()
        {
            Deactivate();
            _button.onClick.AddListener(OnCLick);
        }
        
        private void OnCLick()
        {
            if (_state) Deactivate();
            else Activate();
        }

        private void Deactivate()
        {
            _state = false;
            onDeactivateBooster?.Invoke();
            _button.image.sprite = disableSprite;
        }
        private void Activate()
        {
            _state = true;
            onActivateBooster?.Invoke();
            _button.image.sprite = enableSprite;
        }
    }
}