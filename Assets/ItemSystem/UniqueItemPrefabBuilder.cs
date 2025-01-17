﻿using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ItemSystem
{
#if UNITY_EDITOR

    public class UniqueItemPrefabBuilder : MonoBehaviour
    {
        [SerializeField] Item basePrefab;
        ItemData itemData;
        string uniqueItemHolderPath = "Assets/ItemSystem/ItemSystemScriptables/UniqueItems/UniqueItems_Tier0";

        public List<Type> itemTypes = new()
        {
            typeof(Sword),
            typeof(Boots)
        };


        /// <summary>
        /// Creates a prefab based on the given parameters.
        /// </summary>
        /// <param name="_name"></param>
        /// <param name="_type"></param>
        /// <param name="_mesh"></param>
        /// <param name="_material"></param>
        /// <param name="_uniqueTier"></param>
        /// <param name="_savePath"></param>
        /// <param name="_baseDamage"></param>
        /// <param name="_baseAttackSpeed"></param>
        /// <param name="_baseDefence"></param>
        public void CreatePrefab(string _name, Type _type, Mesh _mesh, Material _material, int _uniqueTier, string _savePath, int _baseDamage = 0, int _baseAttackSpeed = 0, int _baseDefence = 0)
        {
            var newUniqueItem = PrefabUtility.SaveAsPrefabAsset(basePrefab.gameObject, _savePath + _name + ".prefab");
            DestroyImmediate(newUniqueItem.GetComponent<Item>(), true);
            var scriptComponent = newUniqueItem.AddComponent(_type);
            switch (scriptComponent)
            {
                case Weapon weapon:
                    weapon.BaseDamage = _baseDamage;
                    weapon.BaseAttackSpeed = _baseAttackSpeed;
                    newUniqueItem.AddComponent<MeshCollider>();
                    break;
                case Armor armor:
                    armor.BaseDefence = _baseDefence;
                    break;
            }

            scriptComponent.GetComponent<Item>().itemData = itemData;
            newUniqueItem.GetComponent<MeshFilter>().mesh = _mesh;
            newUniqueItem.GetComponent<MeshRenderer>().material = _material;
            newUniqueItem.name = _name;
            var uniqueItemHolder = AssetDatabase.LoadAssetAtPath<UniqueItemHolder>(uniqueItemHolderPath + _uniqueTier + ".asset");
            uniqueItemHolder.UniqueItems.Add(newUniqueItem.GetComponent<Item>());
        }

        /// <summary>
        /// Init Function to generate a new ItemData to be used by the Prefab Generator.
        /// </summary>
        /// <param name="_type"></param>
        /// <param name="_values"></param>
        public void CreateItemData(Type _type, Dictionary<EItemStat, int> _values)
        {
            itemData = new ItemData()
            {
                ItemRarity = EItemRarity.Unique,
                ItemType = ItemTypeDictionaries.GetEItemType(_type)
            };
            foreach (var value in _values)
            {
                itemData.ItemStats[value.Key].StatValue = value.Value;
            }
        }
    }
#endif
}