using DefaultNamespace.Enums;
using Scriptables.SceneLoader;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace LoadingScreen
{
    public class PrepareLoadingScreen : MonoBehaviour
    {
        [SerializeField] EScenes nextScene;
        [SerializeField] NextSceneHolder holder;
        [SerializeField] Sprite backgroundImage;
        public void LoadScene()
        {
            holder.SetNextScene(nextScene);
            holder.backgroundImage = backgroundImage;
            SceneManager.LoadScene("LoadingScreenScene");
        }
    }
}