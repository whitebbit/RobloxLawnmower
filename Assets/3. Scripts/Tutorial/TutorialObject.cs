using UnityEngine;

namespace _3._Scripts.Tutorial
{
    public class TutorialObject : MonoBehaviour
    {
        [SerializeField] private string stepName;
        [SerializeField] private Transform view;
        
        private void Start()
        {
            view.gameObject.SetActive(false);
        }

        private void Awake()
        {
            TutorialSystem.TutorialStepStart += OnTutorialStepStart;
            TutorialSystem.TutorialStepComplete += OnTutorialStepComplete;
        }
        
        private void OnDestroy()
        {
            TutorialSystem.TutorialStepStart -= OnTutorialStepStart;
            TutorialSystem.TutorialStepComplete -= OnTutorialStepComplete;
        }
        
        private void OnTutorialStepStart(string obj)
        {
            if(obj != stepName) return;
            
            view.gameObject.SetActive(true);
            Debug.Log(view.gameObject.activeSelf);
        }
        
        private void OnTutorialStepComplete(string obj)
        {
            if(obj != stepName) return;
            
            view.gameObject.SetActive(false);
            Debug.Log(view.gameObject.activeSelf);
        }
        
        
    }
}