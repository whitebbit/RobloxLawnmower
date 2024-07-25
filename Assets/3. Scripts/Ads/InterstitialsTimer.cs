using System;
using System.Collections;
using _3._Scripts.Config;
using _3._Scripts.Localization;
using _3._Scripts.Singleton;
using GBGamesPlugin;
using UnityEngine;
using UnityEngine.Localization.Components;

namespace _3._Scripts.Ads
{
    public class InterstitialsTimer : Singleton<InterstitialsTimer>
    {
        [SerializeField] private GameObject secondsPanelObject;
        [SerializeField] private LocalizeStringEvent localizedText;

        public bool Active { get; private set; }
        public bool Blocked { get; set; }

        private void Start()
        {
            if (secondsPanelObject)
                secondsPanelObject.SetActive(false);

            if (!Configuration.Instance.InterByTime) return;

            StartCoroutine(CheckTimerAd());
        }

        private IEnumerator CheckTimerAd()
        {
            var checking = true;
            while (checking)
            {
                if (CanShow())
                {
                    _objSecCounter = 2;
                    if (secondsPanelObject)
                        secondsPanelObject.SetActive(true);

                    StartCoroutine(TimerAdShow());
                    yield return checking = false;
                }

                yield return new WaitForSeconds(1.0f);
            }
        }

        private int _objSecCounter = 2;

        private IEnumerator TimerAdShow()
        {
            Active = true;

            while (Active)
            {
                while (_objSecCounter > 0)
                {
                    Debug.Log(_objSecCounter);
                    localizedText.SetVariable("value", _objSecCounter);
                    _objSecCounter--;
                    yield return new WaitForSeconds(1.0f);
                }

                StartCoroutine(BackupTimerClosure());
                GBGames.ShowInterstitial();

                while (!GBGames.NowAdsShow)
                    yield return null;

                secondsPanelObject.SetActive(false);
                _objSecCounter = 2;
                
                StopCoroutine(BackupTimerClosure());
                StartCoroutine(CheckTimerAd());

                Active = false;
            }
        }

        private IEnumerator BackupTimerClosure()
        {
            yield return new WaitForSeconds(2.5f);
            
            secondsPanelObject.SetActive(false);
            _objSecCounter = 2;
            Active = false;
            StopCoroutine(TimerAdShow());
            StartCoroutine(CheckTimerAd());
        }


        private bool CanShow()
        {
            return !GBGames.NowAdsShow && !Blocked && GBGames.CanShowInterstitial;
        }
    }
}