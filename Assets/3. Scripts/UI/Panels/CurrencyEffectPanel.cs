using System;
using _3._Scripts.Currency;
using _3._Scripts.Currency.Enums;
using _3._Scripts.Singleton;
using _3._Scripts.Wallet;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _3._Scripts.UI.Panels
{
    public class CurrencyEffectPanel : Singleton<CurrencyEffectPanel>
    {
        [SerializeField] private RectTransform firstType;
        [SerializeField] private RectTransform secondType;
        [SerializeField] private RectTransform thirdType;

        private RectTransform _rectTransform;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        private void DoEffect(RectTransform obj, CurrencyType type, float count)
        {
            var widget = type switch
            {
                CurrencyType.First => firstType,
                CurrencyType.Second => secondType,
                CurrencyType.Third => thirdType,
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
            obj.DOMove(widget.position, 1f).SetEase(Ease.InBack).OnComplete(() =>
            {                    
                Destroy(obj.gameObject);
                widget.DOScale(1.15f, 0.1f).OnComplete(() =>
                {
                    WalletManager.EarnByType(type, count);
                    widget.DOScale(1, 0.1f);
                });
            });
        }

        public T SpawnEffect<T>(T prefab, CurrencyType type, float count) where T : UIEffect
        {
            var obj = Instantiate(prefab, transform);
            if (obj.transform is not RectTransform objTransform) return obj;

            objTransform.anchoredPosition = GetRandomPosition();
            objTransform.anchorMin = new Vector2(0, 0);
            objTransform.anchorMax = new Vector2(0, 0);

            DoEffect(objTransform, type, count);
            return obj;
        }

        private Vector2 GetRandomPosition()
        {
            var x = Random.Range(0, _rectTransform.rect.width);
            var y = Random.Range(0, _rectTransform.rect.height);
            return new Vector2(x, y);
        }
    }
}