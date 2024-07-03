using System.Collections.Generic;
using LL_Unity_Utils.Lists;
using UnityEngine;

namespace ItemSystem
{
    [CreateAssetMenu(menuName = "Scriptables/Items/UniqueItemHolder", fileName = "NewUniqueItemHolder")]
    public class UniqueItemHolder : ScriptableObject, IWeighable
    {
        public List<Item> UniqueItems;
        public int Tier;
        [field: SerializeField] public int Weight { get; set; }
    }
}