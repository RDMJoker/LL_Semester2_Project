using System;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace ItemSystem
{
    [CreateAssetMenu(menuName = "Scriptables/Items/DropTables/ItemTypeDropTable", fileName = "NewItemTypeDropTable")]
    public class ItemTypeDropTable : ScriptableObject
    {
        [SerializedDictionary("ItemType", "Override Base Weight")]
        public SerializedDictionary<ItemType, int> DropTable = new ();
    }
}