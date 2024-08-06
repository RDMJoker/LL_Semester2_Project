using System;
using System.Collections;
using DefaultNamespace.Enums;
using DG.Tweening;
using Scriptables.SceneLoader;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LoadingScreen
{
    public class LoadScreen : MonoBehaviour
    {
        [SerializeField] float startPositonY;
        [SerializeField] float endPositionY;
        [SerializeField] float animationDuration;

        void Awake()
        {
            DontDestroyOnLoad(gameObject);
            DontDestroyOnLoad(this);
            startPositonY = transform.position.y;
            endPositionY = startPositonY - 1.5f;
        }

        public void StartLoading(SceneLoader _loader)
        {
            StartCoroutine(LoadScene(_loader));
        }

        IEnumerator LoadScene(SceneLoader _scene)
        {
            transform.DOMoveY(endPositionY, animationDuration);
            yield return new WaitForSeconds(animationDuration);
            var sceneLoading = SceneManager.LoadSceneAsync(_scene.scene.ToString());
            // sceneLoading.allowSceneActivation = false;
            yield return new WaitUntil((() => sceneLoading.isDone));
            transform.DOMoveY(startPositonY, animationDuration);
        }
    }
}