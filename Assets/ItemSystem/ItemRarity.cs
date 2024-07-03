using System;
using LL_Unity_Utils.Lists;
using UnityEngine;

namespace ItemSystem
{
    [CreateAssetMenu(menuName = "Scriptables/Items/ItemRarity", fileName = "NewItemRarity")]
    public class ItemRarity : ScriptableObject, IWeighable
    {
        [SerializeField] public EItemRarity Rarity;
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