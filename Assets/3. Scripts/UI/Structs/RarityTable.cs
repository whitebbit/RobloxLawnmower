using System;
using _3._Scripts.UI.Enums;
using UnityEngine;

namespace _3._Scripts.UI.Structs
{
    [Serializable]
    public struct RarityTable
    {
        [SerializeField] private Rarity rarity;
        [SerializeField] private string titleID;
        [SerializeField] private Color mainColor;
        [SerializeField] private Color additionalColor;
        public Rarity Rarity => rarity;
        public string TitleID => titleID;
        public Color MainColor => mainColor;
        public Color AdditionalColor => additionalColor;
    }
}