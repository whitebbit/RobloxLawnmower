using System;
using System.Collections.Generic;
using _3._Scripts.Sounds;
using _3._Scripts.Stages.Scriptable;
using DG.Tweening;
using UnityEngine;
using VInspector;

namespace _3._Scripts.Stages
{
    public class Grass : MonoBehaviour
    {
        [SerializeField] private ParticleSystem particle;
        [SerializeField] private Transform model;
        [SerializeField] private List<MeshRenderer> renderers = new();
        [SerializeField] private List<ParticleSystem> particles = new();

        private Collider _collider;
        public bool Shaved { get; private set; }
        private Vector3 _startScale;
        public GrassData Data { get; private set; }

        private void Awake()
        {
            _startScale = model.localScale;
            _collider = GetComponent<Collider>();
        }

        public void Initialize(GrassData data)
        {
            Data = data;

            foreach (var r in renderers)
            {
                r.material = data.GrassMaterial;
            }

            foreach (var ps in particles)
            {
                var main = ps.main;
                main.startColor = data.GrassParticleColor;
            }
        }

        public void Respawn()
        {
            model.localScale = _startScale;
            model.gameObject.SetActive(true);
            _collider.enabled = true;
            Shaved = false;
        }

        public void CutDown()
        {
            if (Shaved) return;
            particle.Play();
            model.DOScale(Vector3.zero, 0.35f).SetEase(Ease.InOutBack)
                .OnComplete(() => model.gameObject.SetActive(false));
            //_collider.enabled = false;
            Shaved = true;

            SoundManager.Instance.PlayOneShot("cutdownd");
            StageController.Instance.CurrentStage.OnGrassCutDown();
        }
    }
}