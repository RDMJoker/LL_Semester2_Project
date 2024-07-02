using System;
using LL_Unity_Utils.Lists;
using UnityEngine;

namespace ItemSystem
{
    [CreateAssetMenu(menuName = "Scriptables/Items/TierData", fileName = "NewTierData")]
    public class TierData : ScriptableObject, IWeighable
    {
        public EItemStat Stat;
        public EItemType[] CompatibleTypes;
        public float MinValue;
        public float MaxValue;
        public int TierValue;
        [SerializeField] int weight;

        public int Weight
        {
            get => weight;
            set => throw new NotImplementedException();
        }
    }
}