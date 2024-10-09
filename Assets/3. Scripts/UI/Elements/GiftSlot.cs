using System;
using System.Collections.Generic;
using _3._Scripts.UI.Extensions;
using _3._Scripts.UI.Scriptable.Roulette;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VInspector;

namespace _3._Scripts.UI.Elements
{
    public class GiftSlot : MonoBehaviour
    {
        [Tab("Data")]
        [SerializeField] private GiftItem item;
        [Tab("UI")] 
        [SerializeField] private Image icon;
        [SerializeField] private TMP_Text title;
        [SerializeField] private CanvasGroup receiveImage;
        [SerializeField] private Image gotImage;
        
        [Tab("Timer")]
        [SerializeField] private float timeToTake;
        [SerializeField]private Timer timer;
        private bool _rewarded;

        private bool _firstInitialization;
        
        public void Initialize()
        {
            if (!_firstInitialization)
            {
                GetComponent<Button>().onClick.AddListener(GetReward);
                
                timer.StartTimer(timeToTake);
                timer.OnTimerEnd += () =>
                {
                    receiveImage.gameObject.SetActive(true);
                    receiveImage.DOFade(1, 0.25f);

                    timer.gameObject.SetActive(false);
                };
                
                gotImage.Fade(0);
                gotImage.gameObject.SetActive(false);
                receiveImage.alpha =0;
                receiveImage.gameObject.SetActive(false);

                _firstInitialization = true;
            }

            icon.sprite = item.Icon();
            icon.ScaleImage();
            title.text = string.IsNullOrEmpty(item.Title()) ? "" : $"x{item.Title()}";
        }

        private void GetReward()
        {
            if (!timer.TimerEnd()) return;
            if(_rewarded) return;
            
            gotImage.gameObject.SetActive(true);
            gotImage.DOFade(1, 0.15f);
            receiveImage.gameObject.SetActive(false);

            _rewarded = true;
            timer.TimerStopped = true;
            item.OnReward();
        }
    }
}