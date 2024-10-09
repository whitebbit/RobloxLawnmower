using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _3._Scripts.UI;
using _3._Scripts.UI.Panels;
using DG.Tweening;
using UnityEngine;

namespace _3._Scripts.Interactive
{
    public class InteractiveGift : MonoBehaviour
    {
        [SerializeField] private List<Timer> timers = new();
        [SerializeField] private Timer timer;
        [SerializeField] private Transform arrow;
        [SerializeField] private Transform particles;
        
        private int _currentTimer;
        private void Start()
        {
            arrow.gameObject.SetActive(false);
            particles.gameObject.SetActive(false);

            timers = timers.OrderBy(t => t.DurationInSeconds).ToList();
            
            UIManager.Instance.GetPanel<FreeGiftsPanel>().ONOpen += () =>
            {
                arrow.gameObject.SetActive(false);
                particles.gameObject.SetActive(false);
                timer.gameObject.SetActive(true);
            };
            
            StartCoroutine(CheckTimers());
        }

        private int _currentTimerIndex; 
        private float _elapsedTime; 
        private IEnumerator CheckTimers()
        {
            while (_currentTimerIndex < timers.Count)
            {
                var currentTime = timers[_currentTimerIndex].DurationInSeconds - _elapsedTime;

                if (currentTime > 0)
                {
                    timer.StartTimer(currentTime);
                    yield return new WaitForSeconds(currentTime);
                }

                _elapsedTime += currentTime; 

                timer.gameObject.SetActive(false);
                arrow.gameObject.SetActive(true);
                particles.gameObject.SetActive(true);

                _currentTimerIndex++;
            }
            
            arrow.gameObject.SetActive(false);
            particles.gameObject.SetActive(false);
            timer.gameObject.SetActive(false);
        }
        private void OnTriggerEnter(Collider other)
        {
            if(!other.TryGetComponent(out Player.Player _)) return;

            UIManager.Instance.GetPanel<FreeGiftsPanel>().Enabled = true;
            arrow.gameObject.SetActive(false);
            particles.gameObject.SetActive(false);
            timer.gameObject.SetActive(true);
        }
    }
}