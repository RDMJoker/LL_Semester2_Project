using System;
using System.Collections;
using System.Threading;
using Cinemachine;
using DefaultNamespace.Enums;
using DefaultNamespace.Generation;
using LL_Unity_Utils.Generic;
using NaughtyAttributes;
using Unity.Jobs;
using Unity.Jobs.LowLevel.Unsafe;
using UnityEngine;

namespace Generation.DungeonGenerator
{
    public class DungeonGenerator : MonoBehaviour
    {
        [SerializeField] int levelAmount;
        [SerializeField] ScriptableGridListTest gridList;
        [SerializeField] LevelGenerator levelGenerator;
        [SerializeField] LevelGenerator levelGenerator2;

        //TODO: LevelSeed that influences FloorSeed

        [Button]
        void GenerateDungeon()
        {
            for (int i = 1; i < levelAmount + 1; i++)
            {
                levelGenerator.ResetSeed();
                levelGenerator.InitMap(i);
            }
        }

        [Button]
        void GenDungeonMultithreading()
        {
            gridList.ResetEntireList();
            GenerateDungeonJob dungeonJobFirst = new GenerateDungeonJob(1, 10, levelGenerator);
            GenerateDungeonJob dungeonJobSecond = new GenerateDungeonJob(11, 20, levelGenerator2);
            dungeonJobFirst.Execute();
            dungeonJobSecond.Execute();
            var list = gridList.OutputList();
            foreach (var grid in list)
            {
                int roomCount = GetRoomAmountFromGrid(grid);
                Debug.Log("RoomCountOfGrid: " + roomCount);
            }
        }

        int GetRoomAmountFromGrid(ObjectGrid<ERoomTypes> _grid)
        {
            int roomAmount = 0;
            for (int y = 0; y < _grid.Height; y++)
            {
                for (int x = 0; x < _grid.Width; x++)
                {
                    if (_grid.GetValue(x, y) != ERoomTypes.Free) ++roomAmount;
                }
            }

            return roomAmount;
        }
    }
}