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
        [Tab("Follow")] [SerializeField] private Transform target;
        [SerializeField] private Vector3 positionOffset;
        [Tab("Components")] [SerializeField] private MeshRenderer meshRenderer;
        [SerializeField] private GrassSphereDetector grassSphereDetector;

        private Transform _effect;
        private SphereCollider _collider;

        private void Awake()
        {
            _collider = GetComponent<SphereCollider>();
        }

        private void Start()
        {
            grassSphereDetector.OnFound += GrassSphereDetectorOnFound;
        }

        private void OnEnable()
        {
            grassSphereDetector.DetectorState(true);
        }

        private void OnDisable()
        {
            // grassSphereDetector.OnFound -= GrassSphereDetectorOnFound;
            grassSphereDetector.DetectorState(false);
        }

        private void Update()
        {
            var newPosition = target.position + target.forward * positionOffset.x + target.up * positionOffset.y;
            transform.position = newPosition;
            transform.rotation = target.rotation;
        }

        private void GrassSphereDetectorOnFound(Grass obj)
        {
            if (Vector3.Distance(target.position, transform.position) > positionOffset.magnitude) return;
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
            _collider.radius = data.Radius + 0.5f;
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
            if (!other.TryGetComponent(out Grass grass)) return;

            if (grass.Shaved)
                Player.instance.Movement.ResetSpeed();
            else
                Player.instance.Movement.SetSpeed(grass.Data.Resistance);
        }
    }
}