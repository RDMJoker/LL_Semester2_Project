using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ItemSystem
{
    [CreateAssetMenu(menuName = "Scriptables/Items/UniqueItem", fileName = "NewUniqueItem")]
    public class UniqueItemData : ScriptableObject
    {
        public Item prefab;
        public List<StatStruct> statStruct;
        public EItemRarity ItemRarity = EItemRarity.Unique;
        public ItemData itemData;
    }

    [Serializable]
    public struct StatStruct
    {
        public EItemStat stat;
        public StatData statData;
    }
}