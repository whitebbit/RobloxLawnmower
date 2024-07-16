using System;
using GBGamesPlugin;
using UnityEngine;
using UnityEngine.UI;

namespace _3._Scripts.Sounds
{
    [RequireComponent(typeof(Button))]
    public class SoundControllersButton : MonoBehaviour
    {
        [SerializeField] private Image image;
        [SerializeField] private Sprite on;
        [SerializeField] private Sprite off;
        
        
        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(() => ChangeSoundState(!GBGames.saves.sound));
        }

        private void Start()
        {
            ChangeSoundState(GBGames.saves.sound);
        }

        private void ChangeSoundState(bool state)
        {
            GBGames.saves.sound = state;
            AudioListener.volume = state ? 1 : 0;
            image.sprite = state ? on : off;
            GBGames.instance.Save();
        }
    }
}