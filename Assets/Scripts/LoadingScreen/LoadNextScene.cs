using System;
using System.Collections;
using Scriptables.SceneLoader;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace LoadingScreen
{
    public class LoadNextScene : MonoBehaviour
    {
        [SerializeField] NextSceneHolder holder;
        [SerializeField] GameObject wisp;
        [SerializeField] Vector3 endPosition;
        [SerializeField] Image background;
        Vector3 startPosition;

        void Awake()
        {
            background.sprite = holder.backgroundImage;
        }

        void Start()
        {
            startPosition = wisp.transform.position;
            StartCoroutine(LoadScene());
        }

        IEnumerator LoadScene()
        {
            yield return new WaitForSeconds(1f);
            var sceneLoading = SceneManager.LoadSceneAsync((int)holder.nextScene);

            while (!sceneLoading.isDone)
            {
                float progress = Mathf.Clamp01(sceneLoading.progress / 0.9f);
                wisp.transform.position = Vector3.Lerp(startPosition, endPosition, progress);
                yield return null;
            }
        }
    }
}