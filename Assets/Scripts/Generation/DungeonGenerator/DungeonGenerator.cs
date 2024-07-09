using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using DefaultNamespace.Enums;
using LL_Unity_Utils.Generic;
using NaughtyAttributes;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Generation.DungeonGenerator
{
    public class DungeonGenerator : MonoBehaviour
    {
        [SerializeField] int levelAmount;
        [SerializeField] LevelGenerator levelGenerator;
        [SerializeField] bool useMultithreading;
        [SerializeField] int multithreadIterations;

        List<ObjectGrid<ERoomTypes>> objectList01;
        List<long> timeList;

        //TODO: LevelSeed that influences FloorSeed

        [Button]
        List<ObjectGrid<ERoomTypes>> GenerateDungeon()
        {
            List<ObjectGrid<ERoomTypes>> localList = new();
            for (int i = 1; i < levelAmount + 1; i++)
            {
                levelGenerator.ResetSeed();
                localList.Add(levelGenerator.InitMap(i));
            }

            return localList;
        }

        List<ObjectGrid<ERoomTypes>> GenerateDungeon(int _amount)
        {
            List<ObjectGrid<ERoomTypes>> localList = new();
            for (int i = 0; i < _amount; i++)
            {
                levelGenerator.ResetSeed();
                localList.Add(levelGenerator.InitMap(i));
            }

            return localList;
        }

        [Button]
        void GenDungeonMultithreading()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            if (useMultithreading)
            {
                var myTask = new Task<List<ObjectGrid<ERoomTypes>>>(GenerateDungeon);
                myTask.Start();
                var breakoutTimeSpan = new TimeSpan(hours: 0, minutes: 1, seconds: 0);
                myTask.Wait(breakoutTimeSpan);
                if (!myTask.IsCompletedSuccessfully)
                {
                    Debug.LogError("Task had a problem! Please verify integrity of the code!");
                    return;
                }

                objectList01 = myTask.Result;
            }
            else
            {
                objectList01 = GenerateDungeon(levelAmount);
            }

            stopwatch.Stop();
            timeList.Add(stopwatch.ElapsedMilliseconds);

        }

        [Button]
        void MultipleMultithreadingRuns()
        {
            timeList = new List<long>();
            for (int i = 0; i < multithreadIterations; i++)
            {
                GenDungeonMultithreading();
            }

            long additiveTime = timeList.Sum();
            long medianTime = additiveTime / timeList.Count;
            Debug.Log("Generation took an average time of: " + medianTime);
            Debug.Log("First Level Room Count: " + GetRoomAmountFromGrid(objectList01.First()));
            Debug.Log("Fifth Level Room Count " + GetRoomAmountFromGrid(objectList01[5]));
            Debug.Log("Tenth Level Room Count " + GetRoomAmountFromGrid(objectList01[10]));
            Debug.Log("Last Level Room Count: " + GetRoomAmountFromGrid(objectList01.Last()));
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