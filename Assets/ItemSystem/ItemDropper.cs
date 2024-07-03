using System.Collections.Generic;
using System.Linq;
using LL_Unity_Utils.Lists;
using NaughtyAttributes;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ItemSystem
{
    public class ItemDropper : MonoBehaviour
    {
        List<ItemData> items;

        [SerializeField] RandomWeightedList<TierData> tierDatas;
        [SerializeField] RandomWeightedList<UniqueItemHolder> uniqueItems;

        [SerializeField] ItemTypeDropTable defaultItemTypeDropTable;
        [SerializeField] ItemRarityDropTable defaultItemRarityDropTable;
        RandomWeightedList<ItemType> itemTypes = new();
        RandomWeightedList<ItemRarity> itemRarities = new();

        [SerializeField] List<Item> itemPrefabs;
        [SerializeField] Item fallbackPrefab;
        ItemRoller itemRoller;
        WeightedListManager weightedListManager;

        void Awake()
        {
            weightedListManager = new WeightedListManager();
            itemTypes = weightedListManager.SetupItemTypeDropList(defaultItemTypeDropTable);
            itemRarities = weightedListManager.SetupItemRarityDropList(defaultItemRarityDropTable);
            itemTypes.SortList();
            tierDatas.SortList();
            itemRoller = new ItemRoller(tierDatas);
        }

        [Button]
        public void DropItem(int _amount = 1, ItemTypeDropTable _typeDropTable = null, ItemRarityDropTable _rarityDropTable = null)
        {
            itemTypes = weightedListManager.SetupItemTypeDropList(_typeDropTable == null ? defaultItemTypeDropTable : _typeDropTable);
            itemRarities = weightedListManager.SetupItemRarityDropList(_rarityDropTable == null ? defaultItemRarityDropTable : _rarityDropTable);
            items.Clear();
            for (int i = 0; i < _amount; i++)
            {
                var type = GetDroppedType();
                var rarity = GetDroppedRarity();
                if (rarity.Rarity == EItemRarity.Unique)
                {
                    Instantiate(GetRandomUnique(type.Type));
                }
                else
                {
                    var item = itemRoller.RollItem(type, rarity.Rarity);
                    items.Add(item);
                }
            }

            SpawnItems();
        }

        ItemType GetDroppedType()
        {
            return itemTypes.GetRandom();
        }

        ItemRarity GetDroppedRarity()
        {
            return itemRarities.GetRandom();
        }

        void SpawnItems()
        {
            foreach (var itemData in items)
            {
                var prefab = GetPrefab(itemData);
                var droppedItem = Instantiate(prefab);
                droppedItem.itemData = itemData;
            }
        }

        Item GetPrefab(ItemData _itemData)
        {
            var potentialDrops = itemPrefabs.Where(_prefab => _itemData.ItemType == _prefab.itemData.ItemType).ToList();
            if (potentialDrops.Count == 0) potentialDrops.Add(fallbackPrefab);
            return potentialDrops[Random.Range(0, potentialDrops.Count)];
        }

        Item GetRandomUnique(EItemType _type)
        {
            var selectedUniqueTier = uniqueItems.GetRandom();
            var filteredList = selectedUniqueTier.UniqueItems.Where(_item => _item.itemData.ItemType == _type).ToList();
            if (filteredList.Count == 0)
            {
                Debug.LogWarning("No suitable Item found! Spawning different type! Not found type was: " + _type);
                return selectedUniqueTier.UniqueItems[Random.Range(0, selectedUniqueTier.UniqueItems.Count)];
            }

            var selectedUnique = filteredList[Random.Range(0, filteredList.Count)];
            return selectedUnique;
        }
    }
}