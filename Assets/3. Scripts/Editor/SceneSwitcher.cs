using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace _3._Scripts.Editor
{
    public static class SceneSwitcher
    {
        [MenuItem("Scenes/Loader _%#1")] // Ctrl + Shift + 1 (Cmd + Shift + 1 на macOS)
        public static void OpenLoader()
        {
            OpenScene("Loader");
        }

        [MenuItem("Scenes/Main _%#0")] // Ctrl + Shift + 2 (Cmd + Shift + 2 на macOS)
        public static void OpenMainScene()
        {
            OpenScene("MainScene");
        }

        private static void OpenScene(string sceneName)
        {
            var scenePath = "Assets/5. Scenes/" + sceneName + ".unity";
            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                EditorSceneManager.OpenScene(scenePath);
            }
        }
    }
}