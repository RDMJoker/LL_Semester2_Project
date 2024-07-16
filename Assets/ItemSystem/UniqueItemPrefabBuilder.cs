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
        [SerializeField] Mesh mesh;
        [SerializeField] Material material;
        [SerializeField] ItemData itemData;
        [SerializeField] int baseDamage;
        [SerializeField] int baseAttackSpeed;
        [SerializeField] int baseDefence;
        [SerializeField] string uniqueName;
        [SerializeField] int chosenIndex = 0;
        [SerializeField] [Range(0, 3)] int uniqueTier;
        [SerializeField] string testText;
        [SerializeField] int ThisIsAUniqueInt;

        string filePath = "Assets/MyPrefabs/ItemSystem/UniqueItems/";
        string uniqueItemHolderPath = "Assets/ItemSystem/ItemSystemScriptables/UniqueItems/UniqueItems_Tier0";

        List<Type> itemTypes = new()
        {
            typeof(Sword),
            typeof(Boots)
        };


        [Button]
        void CreatePrefab()
        {
            var newUniqueItem = PrefabUtility.SaveAsPrefabAsset(basePrefab.gameObject, filePath + uniqueName + ".prefab");
            DestroyImmediate(newUniqueItem.GetComponent<Item>(), true);
            var scriptComponent = newUniqueItem.AddComponent(itemTypes[chosenIndex]);
            switch (scriptComponent)
            {
                case Weapon weapon:
                    weapon.BaseDamage = baseDamage;
                    weapon.BaseAttackSpeed = baseAttackSpeed;
                    newUniqueItem.AddComponent<MeshCollider>();
                    break;
                case Armor armor:
                    armor.BaseDefence = baseDefence;
                    break;
            }

            scriptComponent.GetComponent<Item>().itemData = itemData;
            newUniqueItem.GetComponent<MeshFilter>().mesh = mesh;
            newUniqueItem.GetComponent<MeshRenderer>().material = material;
            newUniqueItem.name = uniqueName;
            var uniqueItemHolder = AssetDatabase.LoadAssetAtPath<UniqueItemHolder>(uniqueItemHolderPath + uniqueTier + ".asset");
            uniqueItemHolder.UniqueItems.Add(newUniqueItem.GetComponent<Item>());
        }
    }
}