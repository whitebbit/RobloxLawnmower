
using UnityEngine;

namespace _3._Scripts.Detectors.OverlapSystem.Base
{
    public abstract class SphereOverlapDetector<T>: OverlapDetector<T>
    {
        [Header("Sphere settings")] 
        [SerializeField] private float radius;

        public void SetRadius(float radius) => this.radius = radius;
       

        protected override int GetOverlapResult(Vector3 position)
        {
            return Physics.OverlapSphereNonAlloc(position, radius, overlapResults, searchLayer.value);
        }

        protected override void DrawGizmos()
        {
            gizmos.DrawGizmos(offset, radius, startPoint);
        }
    }
}