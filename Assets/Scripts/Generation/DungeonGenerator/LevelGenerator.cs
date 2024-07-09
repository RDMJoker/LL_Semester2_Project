using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using DefaultNamespace.Enums;
using LL_Unity_Utils.Generic;
using NaughtyAttributes;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Generation.DungeonGenerator
{
    public class LevelGenerator : MonoBehaviour
    {
        [SerializeField] int generationWidth;
        [SerializeField] int generationHeight;
        [SerializeField] int minRoomCount;
        [SerializeField] float roomCountIncrementPerLevel;
        [SerializeField] int seed;
        [SerializeField] DungeonBuilder builder;

        int maxRoomCount => (int)((generationWidth + generationHeight) * 0.5f);

        ObjectGrid<ERoomTypes> grid;
        System.Random random;
        List<Vector2Int> endRooms;

        [Button]
        public void ResetSeed()
        {
            seed = 0;
        }

        [Button]
        public ObjectGrid<ERoomTypes> InitMap(int _level = 1)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            if (seed == 0) seed = System.DateTime.Now.Millisecond;
            random = new System.Random(seed);
            endRooms = new List<Vector2Int>();
            grid = new ObjectGrid<ERoomTypes>(generationWidth, generationHeight);
            ResetMap();
            GenerateLevelMap(_level);
            // builder.BuildDungeon(grid);
            Debug.Log(GetLevelLog());
            stopwatch.Stop();
            Debug.Log(stopwatch.ElapsedMilliseconds);
            return grid;
        }

        void ResetMap()
        {
            endRooms.Clear();
            for (int y = 0; y < generationHeight; y++)
            {
                for (int x = 0; x < generationWidth; x++)
                {
                    grid.SetValue(x, y, ERoomTypes.Free);
                }
            }
        }

        void GenerateLevelMap(int _level)
        {
            int roomCountToGenerate = (int)(random.Next(0, 2) + minRoomCount + Mathf.Min(_level * roomCountIncrementPerLevel, maxRoomCount));
            var center = new Vector2Int(generationWidth, generationHeight) / 2;
            bool isValid = false;
            const int maxIterations = 100;
            int currentIterations = 0;
            while (!isValid && currentIterations < maxIterations)
            {
                ResetMap();
                int rooms = GenerateRooms(roomCountToGenerate, center);
                isValid = ValidateMap(roomCountToGenerate, rooms);
                ++currentIterations;
            }

            if (currentIterations >= maxIterations) Debug.LogError("Something went wrong!!" + seed);
            Debug.Log("Generated after " + currentIterations + " iterations!");
            GenerateSpecialRooms();
        }

        bool ValidateMap(int _roomCountToGenerate, int _generatedRooms)
        {
            return _generatedRooms == _roomCountToGenerate && endRooms.Count >= 2;
        }

        void GenerateSpecialRooms()
        {
            var lastRoom = endRooms.Last();
            grid.SetValue(lastRoom, ERoomTypes.Boss);
        }

        int GenerateRooms(int _roomsToGenerate, Vector2Int _center)
        {
            grid.SetValue(_center, ERoomTypes.Normal);

            Queue<Vector2Int> discoveryQueue = new();
            discoveryQueue.Enqueue(_center);

            int generatedRooms = 1;
            while (discoveryQueue.Count > 0)
            {
                var currentCoord = discoveryQueue.Dequeue();
                var neighbourCoords = new[]
                {
                    currentCoord + Vector2Int.right,
                    currentCoord + Vector2Int.left,
                    currentCoord + Vector2Int.up,
                    currentCoord + Vector2Int.down,
                };
                bool hasGenerated = false;

                foreach (var currentNeighbourCoords in neighbourCoords)
                {
                    if (grid.IsOutsideBounds(currentNeighbourCoords)) continue;
                    if (generatedRooms >= _roomsToGenerate) break;
                    if (grid.GetValue(currentNeighbourCoords) != ERoomTypes.Free) continue;
                    if (random.Next(0, 2) == 0) continue;
                    if (GetNeighbourCount(currentNeighbourCoords) > 1) continue;

                    grid.SetValue(currentNeighbourCoords, ERoomTypes.Normal);
                    ++generatedRooms;
                    discoveryQueue.Enqueue(currentNeighbourCoords);
                    hasGenerated = true;
                }

                if (hasGenerated == false) endRooms.Add(currentCoord);
            }

            return generatedRooms;
        }

        int GetNeighbourCount(Vector2Int _coord)
        {
            var neighbourCoords = new Vector2Int[]
            {
                _coord + Vector2Int.right,
                _coord + Vector2Int.left,
                _coord + Vector2Int.up,
                _coord + Vector2Int.down,
            };

            return neighbourCoords.Count(_currentCoord => grid.GetValue(_currentCoord) != ERoomTypes.Free);
        }

        string GetLevelLog()
        {
            var stringBuilder = new StringBuilder();
            for (int y = 0; y < generationHeight; y++)
            {
                stringBuilder.Append("|");
                for (int x = 0; x < generationWidth; x++)
                {
                    int value = (int)grid.GetValue(x, y);
                    if (value >= 0) stringBuilder.Append(" ");
                    stringBuilder.Append(value.ToString() + "|");
                }

                stringBuilder.Append("\n");
            }

            return stringBuilder.ToString();
        }
    }
}