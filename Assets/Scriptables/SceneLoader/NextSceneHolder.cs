using DefaultNamespace.Enums;
using UnityEngine;
using UnityEngine.UI;

namespace Scriptables.SceneLoader
{
    [CreateAssetMenu(menuName = "Scriptables/SceneHolder", fileName = "NewSceneHolder")]
    public class NextSceneHolder : ScriptableObject
    {
        public EScenes nextScene;
        public Sprite backgroundImage;

        public void SetNextScene(EScenes _scene)
        {
            nextScene = _scene;
        }
    }
}