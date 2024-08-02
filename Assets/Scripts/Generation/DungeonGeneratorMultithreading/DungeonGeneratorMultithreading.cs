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

namespace Generation.DungeonGeneratorMultithreading
{
    public class DungeonGeneratorMultithreading : MonoBehaviour
    {
        [SerializeField] int levelAmount;
        [SerializeField] LevelGeneratorMultithreading levelGeneratorMultithreading;
        [SerializeField] bool useMultithreading;
        [SerializeField] int generationIterations;

        List<ObjectGrid<ERoomTypes>> objectList01;
        List<long> timeList;
        List<int> seedList;
        
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
                    var newTask = new Task<ObjectGrid<ERoomTypes>>(() => levelGeneratorMultithreading.InitMap(i1, seedList[i1 - 1]));
                    // secondNewTask.Start();
                    newTask.Start();
                    localList.Add(newTask.Result);
                }
                else
                {
                    levelGeneratorMultithreading.ResetSeed();
                    localList.Add(levelGeneratorMultithreading.InitMap(i, seedList[i - 1]));
                }
            }

            stopWatch.Stop();
            timeList.Add(stopWatch.ElapsedMilliseconds);

            return localList;
        }

        [Button]
        void MultipleGenerationRuns()
        {
            ResetSeedList();
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
        }

        void ResetSeedList()
        {
            seedList = new List<int>();
            for (int i = 0; i < levelAmount; i++)
            {
                seedList.Add(Random.Range(0, 30001));
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