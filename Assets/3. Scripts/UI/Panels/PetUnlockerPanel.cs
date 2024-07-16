using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _3._Scripts.Pets.Scriptables;
using _3._Scripts.UI.Panels.Base;
using _3._Scripts.UI.Structs;
using DG.Tweening;
using GBGamesPlugin;
using UnityEngine;
using UnityEngine.UI;
using VInspector;

namespace _3._Scripts.UI.Panels
{
    public class PetUnlockerPanel : SimplePanel
    {
        [Tab("Main")] [SerializeField] private Image eggImage;
        [SerializeField] private CanvasGroup unlockedPet;
        [Tab("Pet")] [SerializeField] private Image petIcon;
        [SerializeField] private Image glow;
        [Tab("Rarity")] [SerializeField] private List<RarityTable> rarityTables;

        public void UnlockPet(PetData data)
        {
            var rarity = rarityTables.FirstOrDefault(r => r.Rarity == data.Rarity);

            petIcon.sprite = data.Icon;
            glow.color = rarity.MainColor;

            unlockedPet.alpha = 0;
            eggImage.transform.localScale = Vector3.one;
            eggImage.DOFade(1, 0);

            eggImage.transform.DOShakeRotation(1f, 25).OnComplete(() =>
            {
                eggImage.transform.DOScale(2, 0.5f).OnComplete(() =>
                {
                    unlockedPet.DOFade(1, 0.5f).OnComplete(() => { StartCoroutine(DelayDisable()); });
                    unlockedPet.transform.DOScale(0, 0.5f).From().SetEase(Ease.OutBack);
                });
                eggImage.DOFade(0, 0.5f);
            });
            
            GBGames.saves.petsSave.Unlock(data);
        }

        private IEnumerator DelayDisable()
        {
            yield return new WaitForSeconds(1);
            Enabled = false;
        }
    }
}