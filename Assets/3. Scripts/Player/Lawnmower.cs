using System;
using _3._Scripts.Detectors.OverlapSystem;
using _3._Scripts.Player.Scriptables;
using _3._Scripts.Stages;
using DG.Tweening;
using UnityEngine;
using VInspector;

namespace _3._Scripts.Player
{
    public class Lawnmower : MonoBehaviour
    {
        [Tab("Follow")] 
        [SerializeField] private Transform target;
        [SerializeField] private Vector3 positionOffset;
        [Tab("Components")] 
        [SerializeField] private MeshRenderer meshRenderer;
        [SerializeField] private GrassSphereDetector grassSphereDetector;

        private Transform _effect;

        private void OnEnable()
        {
            grassSphereDetector.OnFound += GrassSphereDetectorOnFound;
        }

        private void OnDisable()
        {
            grassSphereDetector.OnFound -= GrassSphereDetectorOnFound;
        }


        private void Update()
        {
            Vector3 newPosition = target.position + target.forward * positionOffset.x + target.up * positionOffset.y;
            transform.position = newPosition;
            transform.rotation = target.rotation;
            
        }

        private void GrassSphereDetectorOnFound(Grass obj)
        {
            if (obj == null) return;
            obj.CutDown();
        }

        public void Initialize(LawnmowerData data)
        {
            InitializeRadius(data);
            InitializeColor(data);
            InitializeEffect(data);
        }

        private void InitializeColor(LawnmowerData data)
        {
            meshRenderer.materials[0].DOColor(data.Color, 0.1f);
        }

        private void InitializeRadius(LawnmowerData data)
        {
            grassSphereDetector.SetRadius(data.Radius);
        }

        private void InitializeEffect(LawnmowerData data)
        {
            if (_effect != null)
            {
                Destroy(_effect.gameObject);
                _effect = null;
            }

            if (data.EffectPrefab == null) return;

            _effect = Instantiate(data.EffectPrefab, transform);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent(out GrassField field)) return;

            Player.instance.Movement.SetSpeed(field.Data.Resistance);
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.TryGetComponent(out GrassField _)) return;

            Player.instance.Movement.ResetSpeed();
        }

    }
}