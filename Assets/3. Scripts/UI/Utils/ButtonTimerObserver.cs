using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace _3._Scripts.UI.Utils
{
    public class ButtonTimerObserver : MonoBehaviour
    {
        [SerializeField] private float minimumCheckTime = 60;

        [SerializeField] private List<Timer> timers = new();
        [SerializeField] private Image notificationImage;

        private Button _button;

        private void Awake()
        {
            _button = GetComponent<Button>();
        }

        private void Start()
        {
            notificationImage.gameObject.SetActive(false);
            _button.onClick.AddListener(() => notificationImage.gameObject.SetActive(false));

            StartCoroutine(CheckTimers());
        }

        private IEnumerator CheckTimers()
        {
            while (true)
            {
                yield return new WaitForSeconds(minimumCheckTime);

                if (timers.Count(timer => timer != null && timer.TimerEnd() && !timer.TimerStopped) > 0)
                    notificationImage.gameObject.SetActive(true);
            }
        }
    }
}