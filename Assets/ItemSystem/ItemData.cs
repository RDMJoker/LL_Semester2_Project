using System;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace ItemSystem
{
    [Serializable]
    public class ItemData
    {
        // public Dictionary<EItemStat, StatData> ItemStats = new()
        // {
        //     { EItemStat.Health , new StatData()},
        //     { EItemStat.Mana , new StatData()},
        //     { EItemStat.AttackSpeed , new StatData()},
        //     { EItemStat.DamageFlat , new StatData()},
        //     { EItemStat.MovementSpeed , new StatData()},
        //     { EItemStat.DamagePercent, new StatData()}
        // }; 
        
        public EItemRarity ItemRarity;
        public EItemType ItemType;

        [SerializedDictionary("ItemStat", "StatData")]
        public SerializedDictionary<EItemStat, StatData> ItemStats = new()
        {
            { EItemStat.Health, new StatData() },
            { EItemStat.Mana, new StatData() },
            { EItemStat.AttackSpeed, new StatData() },
            { EItemStat.DamageFlat, new StatData() },
            { EItemStat.MovementSpeed, new StatData() },
            { EItemStat.DamagePercent, new StatData() }
        };

    }
}