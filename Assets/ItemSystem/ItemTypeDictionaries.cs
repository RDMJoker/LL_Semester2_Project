using System;
using System.Collections.Generic;
using System.Linq;

namespace ItemSystem
{
    public static class ItemTypeDictionaries
    {
        static readonly Dictionary<Type, Type[]> itemTypes = new Dictionary<Type, Type[]>()
        {
            {typeof(Weapon), new []
            {
                typeof(Sword)
            }},
            {typeof(Armor), new []
            {
                typeof(Boots)
            }}
        };

        static Dictionary<EItemType, Type[]> eItemTypesDictionary = new Dictionary<EItemType, Type[]>()
        {
            { EItemType.OneHandedWeapon, new []
            {
                typeof(Sword)
            }},
            { EItemType.Boots, new []
            {
                typeof(Boots)
            } }
        };

        public static bool IsWeapon(Type _type)
        {
            return itemTypes[typeof(Weapon)].Contains(_type);
        }

        public static EItemType GetEItemType(Type _type)
        {
            foreach (var element in eItemTypesDictionary.Where(_element => _element.Value.Contains(_type)))
            {
                return element.Key;
            }

            return EItemType.Debug;
        }
    }
}