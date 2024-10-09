
using System.Collections.Generic;
using UnityEngine;

namespace _3._Scripts.Characters
{
    [ExecuteInEditMode]
    public class Character : MonoBehaviour
    {
        [SerializeField]private Animator animator;
        [SerializeField] private List<SkinnedMeshRenderer> renderers = new();

#if UNITY_EDITOR
        private void OnValidate()
        {
            //animator = GetComponent<Animator>();
        }
#endif

        public void Initialize(Material skin)
        {
            foreach (var meshRenderer in renderers)
            {
                meshRenderer.material = skin;
            }
            Player.Player.instance.PlayerAnimator.SetAvatar(animator.avatar);
        }
    }
}