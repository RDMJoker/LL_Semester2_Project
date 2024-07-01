using System;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;

namespace ItemSystem
{
    public class ItemDropper : MonoBehaviour
    {
        ItemRoller itemRoller;
        public List<ItemData> items;
        
        [SerializeField] int amountToGenerate;
        
        void Awake()
        {
            itemRoller = new ItemRoller();
        }

        [Button]
        void GenerateItems()
        {
            items.Clear();
            for (int i = 0; i < amountToGenerate; i++)
            {
                var item = itemRoller.RollItem();
                items.Add(item);
            }
            LogList();
        }

        void LogList()
        {
            int commonCount = items.Count(_item => _item.ItemRarity == EItemRarity.Common);
            int uncommonCount = items.Count(_item => _item.ItemRarity == EItemRarity.Uncommon);
            int rareCount = items.Count(_item => _item.ItemRarity == EItemRarity.Rare);
            int legendaryCount = items.Count(_item => _item.ItemRarity == EItemRarity.Legendary);
            int uniqueCount = items.Count(_item => _item.ItemRarity == EItemRarity.Unique);
            Debug.Log(commonCount);
            Debug.Log(uncommonCount);
            Debug.Log(rareCount);
            Debug.Log(legendaryCount);
            Debug.Log(uniqueCount);
        }
    }
}