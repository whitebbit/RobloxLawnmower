using System.Linq;
using _3._Scripts.Config;
using UnityEngine;

namespace _3._Scripts.Characters
{
    public class CharacterHandler
    {
        public Character Current { get; private set; }

        public void SetCharacter(string id, Transform parent)
        {
            DeleteCharacter();
            SpawnCharacter(id, parent);
        }

        private void SpawnCharacter(string id, Transform parent)
        {
            var character = Configuration.Instance.AllCharacters.FirstOrDefault(c => c.ID == id)?.Prefab;
            Current = Object.Instantiate(character, parent);
            Current.transform.localPosition = -Vector3.up;
            Current.Initialize();
        }

        private void DeleteCharacter()
        {
            if (Current == null) return;
            Object.Destroy(Current.gameObject);
        }
    }
}