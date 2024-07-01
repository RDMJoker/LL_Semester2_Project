using System;
using System.Collections.Generic;

namespace ItemSystem
{
    [Serializable]
    public class ItemData
    {
        public Dictionary<EItemStat, float> ItemStats = new()
        {
            { EItemStat.Health , 0},
            { EItemStat.Mana , 0},
            { EItemStat.AttackSpeed , 0},
            { EItemStat.DamageFlat , 0},
            { EItemStat.MovementSpeed , 0},
            { EItemStat.DamagePercent, 0}
        }; 
        public EItemRarity ItemRarity;
    }
}