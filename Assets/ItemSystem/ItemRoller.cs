﻿using System;
using System.Collections.Generic;
using System.Linq;
using LL_Unity_Utils.Lists;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ItemSystem
{
    public class ItemRoller
    {
        ItemData data;
        readonly RandomWeightedList<TierData> tierData;
        EItemStat chosenStat;
        const int MaxElevatedStats = 3;
        const float ElevationChancePercentile = 2;

        public ItemRoller(RandomWeightedList<TierData> _tierDatas)
        {
            tierData = _tierDatas;
        }

        public ItemData RollItem(ItemType _itemType, EItemRarity _rarity)
        {
            data = new ItemData
            {
                ItemRarity = _rarity,
                ItemType = _itemType.Type
            };
            GenerateStats();
            return data;
        }
        void GenerateStats()
        {
            var potentialStats = GetFilteredList();
            for (int i = 0; i <= (int)data.ItemRarity; i++)
            {
                if (potentialStats.Count == 0)
                {
                    foreach (var (currentKey, value) in data.ItemStats.Where(_itemStat => _itemStat.Value.StatValue != 0))
                    {
                        data.ItemStats[currentKey].StatValue = value.StatValue * 2f;
                        data.ItemStats[currentKey].IsElevated = true;
                    }

                    return;
                }

                chosenStat = potentialStats[Random.Range(0, potentialStats.Count)];
                var chosenTierData = GetRandomTierData(chosenStat);
                data.ItemStats[chosenStat].StatValue = (int)Random.Range(chosenTierData.MinValue, chosenTierData.MaxValue);
                if (ShouldElevate()) Elevate(chosenStat);
                data.ItemStats[chosenStat].TierValue = chosenTierData.TierValue;

                potentialStats.RemoveAll(ContainsChosenStat);
            }
        }

        bool ContainsChosenStat(EItemStat _itemStat)
        {
            return _itemStat == chosenStat;
        }

        bool ShouldElevate()
        {
            int elevatedStats = 0;
            foreach (var statData in data.ItemStats.Values.Where(_statData => elevatedStats <= MaxElevatedStats).Where(_statData => _statData.IsElevated))
            {
                elevatedStats++;
            }

            if (elevatedStats >= MaxElevatedStats) return false;
            int randomNumber = Random.Range(0, 101);
            return randomNumber <= ElevationChancePercentile;
        }

        void Elevate(EItemStat _chosenStat)
        {
            data.ItemStats[_chosenStat].StatValue *= 2;
            data.ItemStats[_chosenStat].IsElevated = true;
        }

        List<EItemStat> GetFilteredList()
        {
            return tierData.weightedList.Where(_tier => _tier.CompatibleTypes.Contains(data.ItemType)).Select(_tier => _tier.Stat).ToList();
        }

        TierData GetRandomTierData(EItemStat _chosenStat)
        {
            RandomWeightedList<TierData> filteredTierData = new();
            foreach (var tier in tierData.weightedList.Where(_tier => _tier.Stat == _chosenStat))
            {
                filteredTierData.Add(tier);
            }

            if (filteredTierData.weightedList.Count == 0) Debug.LogError("No fitting stat found! Please check if everything is setup correctly!");
            else
            {
                filteredTierData.SortList();
                return filteredTierData.GetRandom();
            }

            Debug.LogWarning("Something went wrong! Default TierData got returned!");
            return default;
        }
    }
}