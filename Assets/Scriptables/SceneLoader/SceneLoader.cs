using DefaultNamespace.Enums;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scriptables.SceneLoader
{
    [CreateAssetMenu(menuName = "Scriptables/SceneLoader", fileName = "NewSceneLoader")]
    public class SceneLoader : ScriptableObject
    {
        public EScenes scene;

        public void Invoke()
        {
            SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
            SceneManager.LoadScene((int)scene);
        }
    }
}