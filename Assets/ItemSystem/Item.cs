using System.Linq;
using System.Reflection;
using UnityEditor.TextCore.Text;
using UnityEngine;

namespace ItemSystem
{
    public class Item : MonoBehaviour
    {
        public ItemData itemData;

        protected virtual void Start()
        {
            Debug.Log(itemData.ItemType);
            Debug.Log(itemData.ItemRarity);
            foreach (var itemStat in itemData.ItemStats.Where(_itemStat => _itemStat.Value.StatValue != 0))
            {
                Debug.Log(itemStat.Key + " || " + itemStat.Value);
            }
        }
    }
}