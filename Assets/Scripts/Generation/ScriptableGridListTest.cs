using System.Collections.Generic;
using DefaultNamespace.Enums;
using LL_Unity_Utils.Generic;
using UnityEngine;

namespace DefaultNamespace.Generation
{
    [CreateAssetMenu(menuName = "TestScriptables/GridList", fileName = "NewGridList")]
    public class ScriptableGridListTest : ScriptableObject
    {
        List<ObjectGrid<ERoomTypes>> gridList = new();

        public void ResetEntireList()
        {
            gridList.Clear();
        }

        public void AddToList(ObjectGrid<ERoomTypes> _grid)
        {
            gridList.Add(_grid);
        }
        public List<ObjectGrid<ERoomTypes>> OutputList()
        {
            return gridList;
        }
    }
}