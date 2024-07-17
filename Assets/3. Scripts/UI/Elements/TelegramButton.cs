using System;
using _3._Scripts.Config;
using UnityEngine;
using UnityEngine.UI;

namespace _3._Scripts.UI.Elements
{
    public class TelegramButton : MonoBehaviour
    {
        [SerializeField] private RemoteConfig<bool> useButton;
        [SerializeField] private string url;

        private void Start()
        {
            gameObject.SetActive(useButton.Value);
            GetComponent<Button>().onClick.AddListener(() => Application.OpenURL(url));
        }
    }
}