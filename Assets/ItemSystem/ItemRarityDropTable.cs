using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace ItemSystem
{
    [CreateAssetMenu(menuName = "Scriptables/Items/DropTables/ItemRarityDropTable", fileName = "NewItemRarityDropTable")]
    public class ItemRarityDropTable : ScriptableObject
    {
        [SerializedDictionary("ItemRarity", "OverwriteWeight")]
        public SerializedDictionary<ItemRarity, int> DropTable = new();
    }
}