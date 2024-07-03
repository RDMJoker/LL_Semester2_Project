using LL_Unity_Utils.Lists;

namespace ItemSystem
{
    public class WeightedListManager
    {
        RandomWeightedList<ItemType> itemTypes = new();
        RandomWeightedList<ItemRarity> itemRarities = new();
        
       public RandomWeightedList<ItemType> SetupItemTypeDropList(ItemTypeDropTable _dropTable)
        {
            ResetWeights(_dropTable);
            itemTypes = new RandomWeightedList<ItemType>();
            foreach (var data in _dropTable.DropTable)
            {
                if (data.Value == 0 || data.Value == data.Key.Weight) itemTypes.Add(data.Key);
                else
                {
                    data.Key.Weight = data.Value;
                    itemTypes.Add(data.Key);
                }
            }

            itemTypes.SortList();
            return itemTypes;
        }

       public RandomWeightedList<ItemRarity> SetupItemRarityDropList(ItemRarityDropTable _dropTable)
        {
            ResetWeights(null, _dropTable);
            itemRarities = new RandomWeightedList<ItemRarity>();
            foreach (var data in _dropTable.DropTable)
            {
                if (data.Value == 0 || data.Value == data.Key.Weight) itemRarities.Add(data.Key);
                else
                {
                    data.Key.Weight = data.Value;
                    itemRarities.Add(data.Key);
                }
            }

            itemRarities.SortList();
            return itemRarities;
        }


        void ResetWeights(ItemTypeDropTable _itemTypeDropTable = null, ItemRarityDropTable _itemRarityDropTable = null)
        {
            if (_itemTypeDropTable != null)
            {
                foreach (var data in _itemTypeDropTable.DropTable)
                {
                    data.Key.Weight = data.Key.BaseWeight;
                }
            }

            if (_itemRarityDropTable != null)
            {
                foreach (var data in _itemRarityDropTable.DropTable)
                {
                    data.Key.Weight = data.Key.BaseWeight;
                }
            }
        }
    }
}