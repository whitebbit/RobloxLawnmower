using System.Linq;
using _3._Scripts.Config;
using UnityEngine;

namespace _3._Scripts.Characters
{
    public class CharacterHandler
    {
        public Character Current { get; private set; }

        public CharacterHandler(Character character)
        {
            Current = character;
        }

        public void SetCharacter(string id)
        {
            Initialize(id);
        }

        private void Initialize(string id)
        {
            var character = Configuration.Instance.AllCharacters.FirstOrDefault(c => c.ID == id);
            if (character is not null) 
                Current.Initialize(character.Skin);
        }
    }
}