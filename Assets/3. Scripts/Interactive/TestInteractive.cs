using _3._Scripts.Interactive.Interfaces;
using UnityEngine;

namespace _3._Scripts.Interactive
{
    public class TestInteractive : MonoBehaviour, IInteractive
    {
        public void StartInteract()
        {
            Debug.Log($"{gameObject.name} start");
        }

        public void Interact()
        {
            Debug.Log($"{gameObject.name} Interact");
        }

        public void StopInteract()
        {
            Debug.Log($"{gameObject.name} stop");
        }
    }
}