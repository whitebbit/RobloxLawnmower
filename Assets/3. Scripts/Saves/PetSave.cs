using System;
using System.Collections.Generic;
using System.Linq;
using _3._Scripts.Pets.Scriptables;
using UnityEngine;

namespace _3._Scripts.Saves
{
    [Serializable]
    public class PetSave
    {
        public List<PetSaveData> unlocked = new();
        public List<PetSaveData> selected = new();
        public event Action ONPetUnlocked;
        public void Unlock(PetData data)
        {
            var hash = $"{data.ID}_{DateTime.Now}".GetHashCode();
            var petSaveData = new PetSaveData
            {
                dataID = data.ID,
                booster = data.RandomBooster,
                id = hash
            };

            unlocked.Add(petSaveData);
            ONPetUnlocked?.Invoke();
        }

        public void Select(int id)
        {
            var pet = unlocked.FirstOrDefault(p => p.id == id);
            if (Selected(id) || !Unlocked(id)) return;

            selected.Add(pet);
        }

        public void Unselect(int id)
        {
            var pet = selected.FirstOrDefault(p => p.id == id);
            if (!Selected(id)) return;
            
            selected.Remove(pet);
        }

        public void Remove(int id)
        {
            var pet = unlocked.FirstOrDefault(p => p.id == id);
            Unselect(id);
            unlocked.Remove(pet);
        }
        
        public bool MaxSelected(int maxObjects)
        {
            return selected.Count >= maxObjects;
        }
        
        public bool MaxUnlocked(int maxObjects)
        {
            return unlocked.Count >= maxObjects;
        }

        public bool Unlocked(int id) => unlocked.Exists(p => p.id == id);
        public bool Selected(int id) => selected.Exists(p => p.id == id);
    }
}