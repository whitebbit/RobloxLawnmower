using System;
using System.Collections;
using _3._Scripts.Boosters;
using _3._Scripts.Config;
using _3._Scripts.Currency.Enums;
using _3._Scripts.Localization;
using _3._Scripts.Singleton;
using _3._Scripts.UI;
using _3._Scripts.UI.Effects;
using _3._Scripts.UI.Elements;
using _3._Scripts.UI.Panels;
using _3._Scripts.Wallet;
using DG.Tweening;
using GBGamesPlugin;
using UnityEngine;
using UnityEngine.Localization.Components;

namespace _3._Scripts.Ads
{
    public class InterstitialsTimer : Singleton<InterstitialsTimer>
    {

        [SerializeField] private CanvasGroup secondsPanelObject;
        [SerializeField] private LocalizeStringEvent localizedText;
        [SerializeField] private CurrencyCounterEffect counterEffect;

        public bool Active { get; private set; }
        public bool Blocked { get; set; }

        private void Start()
        {
            secondsPanelObject.alpha = 0;
            
            if (!Configuration.Instance.InterByTime) return;
            
            GBGames.InterstitialClosedCallback += Reward;
            StartCoroutine(CheckTimerAd());
        }

        private void Reward()
        {
            var effectInstance = CurrencyEffectPanel.Instance.SpawnEffect(counterEffect, CurrencyType.Second, 25);
            effectInstance.Initialize(CurrencyType.Second, 25);
        }

        private IEnumerator CheckTimerAd()
        {
            var checking = true;
            while (checking)
            {
                if (CanShow())
                {
                    _objSecCounter = 3;
                    if (secondsPanelObject)
                        secondsPanelObject.DOFade(1, 0.25f);

                    _currentCoroutine = StartCoroutine(TimerAdShow());
                    yield return checking = false;
                }

                yield return new WaitForSeconds(1.0f);
            }
        }

        private int _objSecCounter = 3;
        private Coroutine _backupCoroutine;
        private Coroutine _currentCoroutine;

        private IEnumerator TimerAdShow()
        {
            Active = true;

            while (Active)
            {
                while (_objSecCounter > 0)
                {
                    localizedText.SetVariable("value", _objSecCounter);
                    _objSecCounter--;
                    yield return new WaitForSeconds(1.0f);
                }

                _backupCoroutine = StartCoroutine(BackupTimerClosure());
                GBGames.ShowInterstitial();

                while (!GBGames.NowAdsShow)
                    yield return null;
                
                secondsPanelObject.alpha = 0;
                _objSecCounter = 3;

                StopCoroutine(_backupCoroutine);
                StartCoroutine(CheckTimerAd());

                Active = false;
            }
        }

        private IEnumerator BackupTimerClosure()
        {
            yield return new WaitForSeconds(2.5f);

            secondsPanelObject.alpha = 0;
            _objSecCounter = 3;
            Active = false;
            StopCoroutine(_currentCoroutine);
            StartCoroutine(CheckTimerAd());
        }


        private bool CanShow()
        {
            return !GBGames.NowAdsShow && !Blocked && GBGames.CanShowInterstitial && !UIManager.Instance.Active;
        }
    }
}