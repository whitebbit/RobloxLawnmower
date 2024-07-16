using System;
using UnityEngine;
using VInspector;

namespace _3._Scripts.Stages
{
    public class Grass : MonoBehaviour
    {
        [SerializeField] private ParticleSystem particle;
        [SerializeField] private Transform model;

        private Collider _collider;

        private void Awake()
        {
            _collider = GetComponent<Collider>();
        }
        public void Respawn()
        {
            model.gameObject.SetActive(true);
            _collider.enabled = true;
        }

        public void CutDown()
        {
            particle.Play();
            model.gameObject.SetActive(false);
            _collider.enabled = false;
        }
    }
}