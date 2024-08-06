using DefaultNamespace.Enums;
using Scriptables.SceneLoader;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace LoadingScreen
{
    public class PrepareLoadingScreen : MonoBehaviour
    {
        [SerializeField] SceneLoader loader;
        [SerializeField] LoadScreen loadScreen;
        public void LoadScene()
        {
            loadScreen.StartLoading(loader);
        }
    }
}