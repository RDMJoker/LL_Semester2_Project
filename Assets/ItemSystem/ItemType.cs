using LL_Unity_Utils.Lists;
using UnityEngine;

namespace ItemSystem
{
    [CreateAssetMenu(menuName = "Scriptables/Items/ItemType", fileName = "NewItemType")]
    public class ItemType : ScriptableObject, IWeighable
    {
        [SerializeField] public EItemType Type;
        [SerializeField] int weight;
        public int Weight
        {
            get => weight;
            set => throw new System.NotImplementedException();
        }
    }
}