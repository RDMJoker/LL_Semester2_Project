using System.Collections.Generic;
using System.Linq;
using LL_Unity_Utils.Lists;
using NaughtyAttributes;
using UnityEngine;

namespace ItemSystem
{
    public class ItemDropper : MonoBehaviour
    {
        /* Only temporary SerializeField, to Debug better */
        [SerializeField] List<ItemData> items;
        
        /* TODO: Replace with random Drop amount, but keep different option for debugging */
        [SerializeField] int amountToGenerate;

        [SerializeField] RandomWeightedList<ItemType> itemTypes;
        [SerializeField] RandomWeightedList<TierData> tierDatas;
        [SerializeField] RandomWeightedList<UniqueItemHolder> uniqueItems;
        

        [SerializeField] List<Item> itemPrefabs;
        [SerializeField] Item debugPrefab;
        ItemRoller itemRoller;
        Item rolledUnique;

        void Awake()
        {
            // itemTypes = new RandomWeightedList<ItemType>();
            itemTypes.SortList();
            tierDatas.SortList();
            itemRoller = new ItemRoller(tierDatas,uniqueItems);
        }

        [Button]
        void GenerateItems()
        {
            rolledUnique = null;
            items.Clear();
            for (int i = 0; i < amountToGenerate; i++)
            {
                var item = itemRoller.RollItem(GetDroppedType(), out var uniqueItem);
                if (uniqueItem != null) rolledUnique = uniqueItem.prefab;
                items.Add(item);
            }
            DropItems();
            LogList();
        }

        ItemType GetDroppedType()
        {
            return itemTypes.GetRandom();
        }

        void DropItems()
        {
            if (rolledUnique != null)
            {
                var droppedItem = Instantiate(rolledUnique);
            }
            foreach (var itemData in items)
            {
                var prefab = GetPrefab(itemData);
                var droppedItem = Instantiate(rolledUnique != null ? rolledUnique : prefab);
                droppedItem.itemData = itemData;
            }
        }

        Item GetPrefab(ItemData _itemData)
        {
            var potentialDrops = itemPrefabs.Where(_prefab => _itemData.ItemType == _prefab.BaseType).ToList();
            if(potentialDrops.Count == 0) potentialDrops.Add(debugPrefab);
            return potentialDrops[Random.Range(0, potentialDrops.Count)];
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