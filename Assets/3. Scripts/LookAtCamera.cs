using UnityEngine;

namespace _3._Scripts
{
    public class LookAtCamera : MonoBehaviour
    {
        private Transform _target; 

        private void Start()
        {
           _target = CameraController.Instance.MainCamera; 
        }

        private void Update()
        {
            transform.LookAt(_target);
        }
    }
}