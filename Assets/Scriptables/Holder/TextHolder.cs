using UnityEngine;

namespace Scriptables.Holder
{
    [CreateAssetMenu(menuName = "Scriptables/Holder/TextHolder", fileName = "NewTextHolder")]
    public class TextHolder : ScriptableObject
    {
        public string header;

        [TextArea(1, 10)] public string description;
    }
}