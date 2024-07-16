using System.Collections.Generic;
using System.Linq;
using _3._Scripts.Config;
using _3._Scripts.Pets.Scriptables;
using _3._Scripts.Saves;
using UnityEngine;

namespace _3._Scripts.Pets
{
    public class PetsHandler
    {
        public List<Pet> Pets { get; } = new();

        public void AddPet(Pet obj) => Pets.Add(obj);

        public void CreatePet(PetSaveData saveData, Vector3 position)
        {
            var data = Configuration.Instance.GetPet(saveData.dataID);
            var pet = Object.Instantiate(data.Prefab);
            pet.transform.position = position;
            pet.SetData(saveData);
            AddPet(pet);
        }

        public void RemovePet(Pet pet) => Pets.Remove(pet);

        public void DestroyPet(int id)
        {
            var pet = Pets.FirstOrDefault(p => p.Data.id == id);
            if (pet == null) return;

            RemovePet(pet);
            Object.Destroy(pet.gameObject);
        }

        public void SetState(bool state)
        {
            foreach (var pet in Pets)
            {
                pet.gameObject.SetActive(state);
            }
        }
    }
}