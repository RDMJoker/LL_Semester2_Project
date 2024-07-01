using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

namespace ItemSystem
{
    public class ItemRoller
    {
        ItemData data;

        public ItemData RollItem()
        {
            data = new ItemData
            {
                ItemRarity = GenerateRarity(),
            };
            GenerateStats();
            return data;
        }

        EItemRarity GenerateRarity()
        {
            float random = Random.Range(0f, 100f);
            return random switch
            {
                < RarityWeights.UniqueWeightThreshhold => EItemRarity.Unique,
                < RarityWeights.LegendaryWeightThreshhold => EItemRarity.Legendary,
                < RarityWeights.RareWeightThreshhold => EItemRarity.Rare,
                < RarityWeights.UncommonWeightThreshhold => EItemRarity.Uncommon,
                <= RarityWeights.CommonWeightThreshhold => EItemRarity.Common,
                _ => throw new NotImplementedException()
            };
        }

        void GenerateStats()
        {
            List<EItemStat> potentialStats = new()
            {
                EItemStat.Health,
                EItemStat.Mana,
                EItemStat.AttackSpeed,
                EItemStat.DamageFlat,
                EItemStat.DamagePercent,
                EItemStat.MovementSpeed
            };
            for (int i = 0; i <= (int)data.ItemRarity; i++)
            {
                var chosenStat = potentialStats[Random.Range(0, potentialStats.Count)];
                data.ItemStats[chosenStat] = Random.Range(1, 11);
                potentialStats.Remove(chosenStat);
            }
        }
    }
}