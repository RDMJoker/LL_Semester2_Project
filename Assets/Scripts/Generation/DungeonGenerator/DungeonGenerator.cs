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
using Random = UnityEngine.Random;

namespace Generation.DungeonGenerator
{
    public class DungeonGenerator : MonoBehaviour
    {
        [SerializeField] int levelAmount;
        [SerializeField] LevelGenerator levelGenerator;
        [SerializeField] bool useMultithreading;
        [SerializeField] int generationIterations;

        List<ObjectGrid<ERoomTypes>> objectList01;
        List<long> timeList;
        List<int> seedList;

        //TODO: LevelSeed that influences FloorSeed

        [Button]
        List<ObjectGrid<ERoomTypes>> GenerateDungeon()
        {
            var stopWatch = new Stopwatch();
            List<ObjectGrid<ERoomTypes>> localList = new();
            stopWatch.Start();
            for (int i = 1; i < levelAmount + 1; i++)
            {
                if (useMultithreading)
                {
                    // levelGenerator.ResetSeed();
                    int i1 = i;
                    // var secondNewTask = new Task(levelGenerator.ResetSeed);
                    var newTask = new Task<ObjectGrid<ERoomTypes>>(() => levelGenerator.InitMap(i1, seedList[i1 - 1]));
                    // secondNewTask.Start();
                    newTask.Start();
                    localList.Add(newTask.Result);
                }
                else
                {
                    levelGenerator.ResetSeed();
                    localList.Add(levelGenerator.InitMap(i, seedList[i - 1]));
                }
            }

            stopWatch.Stop();
            timeList.Add(stopWatch.ElapsedMilliseconds);

            return localList;
        }

        [Button]
        void MultipleGenerationRuns()
        {
            if (seedList == null)
            {
                seedList = new List<int>();
                for (int i = 0; i < levelAmount; i++)
                {
                    seedList.Add(Random.Range(0, 30001));
                }
            }

            timeList = new List<long>();
            for (int i = 0; i < generationIterations; i++)
            {
                objectList01 = GenerateDungeon();
            }

            long additiveTime = timeList.Sum();
            long medianTime = additiveTime / timeList.Count;
            timeList.Sort();
            long minTime = timeList.First();
            long maxTime = timeList.Last();
            Debug.Log("Generation took an average time of: " + medianTime);
            Debug.Log("The shortest generation took: " + minTime);
            Debug.Log("The longest generation took: " + maxTime);
            // Debug.Log("First Level Room Count: " + GetRoomAmountFromGrid(objectList01.First()));
            // Debug.Log("Fifth Level Room Count " + GetRoomAmountFromGrid(objectList01[5]));
            // Debug.Log("Tenth Level Room Count " + GetRoomAmountFromGrid(objectList01[10]));
            // Debug.Log("Last Level Room Count: " + GetRoomAmountFromGrid(objectList01.Last()));
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