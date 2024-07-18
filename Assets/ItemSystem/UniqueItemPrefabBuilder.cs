using System;
using System.Collections.Generic;
using System.IO;
using NaughtyAttributes;
using UnityEditor;
using UnityEngine;

namespace ItemSystem
{
    public class UniqueItemPrefabBuilder : MonoBehaviour
    {
        [SerializeField] Item basePrefab;

        // [SerializeField] Mesh mesh;
        // [SerializeField] Material material;
        ItemData itemData;
        // [SerializeField] int baseDamage;
        // [SerializeField] int baseAttackSpeed;
        // [SerializeField] int baseDefence;
        // [SerializeField] string uniqueName;
        // [SerializeField] int chosenIndex = 0;
        // [SerializeField] [Range(0, 3)] int uniqueTier;

        string filePath = "Assets/MyPrefabs/ItemSystem/UniqueItems/";
        string uniqueItemHolderPath = "Assets/ItemSystem/ItemSystemScriptables/UniqueItems/UniqueItems_Tier0";

        public List<Type> itemTypes = new()
        {
            typeof(Sword),
            typeof(Boots)
        };


        public void CreatePrefab(string _name, Type _type, Mesh _mesh, Material _material, int _uniqueTier, int _baseDamage = 0, int _baseAttackSpeed = 0, int _baseDefence = 0)
        {
            var newUniqueItem = PrefabUtility.SaveAsPrefabAsset(basePrefab.gameObject, filePath + _name + ".prefab");
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

        public void CreateItemData(Type _type, Dictionary<EItemStat, int> _values)
        {  
            itemData = new ItemData()
            {
                ItemRarity = EItemRarity.Unique,
                ItemType = ItemTypeDictionaries.GetEItemType(_type)
            };
        }
    }
}