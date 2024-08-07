using System;
using System.Collections;
using DefaultNamespace.Enums;
using DG.Tweening;
using Scriptables.SceneLoader;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LoadingScreen
{
    public class LoadScreen : MonoBehaviour
    {
        [SerializeField] float startPositionY;
        [SerializeField] float endPositionY;
        [SerializeField] float animationDuration;
        [SerializeField] Canvas myCanvas;
        [SerializeField] TextMeshProUGUI loadingScreenText;
        
        
        void Awake()
        {
            DontDestroyOnLoad(gameObject);
            DontDestroyOnLoad(this);
            DontDestroyOnLoad(myCanvas);
            startPositionY += 540f;
            endPositionY += 540f;
        }

        public void OverwriteText(string _text)
        {
            loadingScreenText.text = _text;
        }
        public void StartLoading(SceneLoader _loader)
        {
            StartCoroutine(LoadScene(_loader));
        }

        IEnumerator LoadScene(SceneLoader _scene)
        {
            Cursor.visible = false;
            transform.DOMoveY(endPositionY, animationDuration);
            yield return new WaitForSeconds(animationDuration);
            var sceneLoading = SceneManager.LoadSceneAsync(_scene.scene.ToString());
            // sceneLoading.allowSceneActivation = false;
            yield return new WaitUntil((() => sceneLoading.isDone));
            transform.DOMoveY(startPositionY, animationDuration);
            yield return new WaitForSeconds(animationDuration);
            Cursor.visible = true;
        }
    }
}