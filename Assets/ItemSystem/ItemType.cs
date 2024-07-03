using System;
using LL_Unity_Utils.Lists;
using UnityEngine;

namespace ItemSystem
{
    [Serializable]
    [CreateAssetMenu(menuName = "Scriptables/Items/ItemType", fileName = "NewItemType")]
    public class ItemType : ScriptableObject, IWeighable
    {
        [SerializeField] public EItemType Type;
        [SerializeField] int weight;
        public int BaseWeight;
        public int Weight
        {
            get => weight;
            set => weight = value;
        }

        void OnValidate()
        {
            BaseWeight = weight;
        }
    }
}