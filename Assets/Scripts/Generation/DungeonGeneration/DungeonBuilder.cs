using System;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace.Enums;
using Generation.DungeonGeneration.DungeonGenerationScriptables;
using LL_Unity_Utils.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace Generation.DungeonGeneration
{
    public class DungeonBuilder : MonoBehaviour
    {
        Transform groupParent;


        List<GameObject> roomObjects = new();
        ObjectGrid<ERoomTypes> grid;
        GenerationTileset tileset;

        Room currentRoom;

        //public void BuildDungeon(ObjectGrid<ERoomTypes> _levelGrid, int _yOffset = 0)
        // {
        //     roomObjects ??= new List<GameObject>();
        //     ResetDungeon();
        //     for (int y = 0; y < _levelGrid.Height; y++)
        //     {
        //         for (int x = 0; x < _levelGrid.Width; x++)
        //         {
        //             if (_levelGrid.GetValue(x, y) == ERoomTypes.Normal) roomObjects.Add(Instantiate(prefab, new Vector3(x, _yOffset, y), Quaternion.identity));
        //             if (_levelGrid.GetValue(x, y) != ERoomTypes.Boss) continue;
        //             var bossRoom = Instantiate(prefab, new Vector3(x, 0, y), Quaternion.identity);
        //             bossRoom.TryGetComponent(out MeshRenderer bossSpriteRenderer);
        //             bossSpriteRenderer.material = new Material(bossSpriteRenderer.material)
        //             {
        //                 color = Color.red
        //             };
        //             roomObjects.Add(bossRoom);
        //         }
        //     }
        // }

        public void BuildDungeon(ObjectGrid<ERoomTypes> _levelGrid, GenerationTileset _tileset, Transform _groupParent, int _yOffset = 0)
        {
            grid = _levelGrid;
            tileset = _tileset;
            groupParent = _groupParent;
            roomObjects ??= new List<GameObject>();
            for (int y = 0; y < grid.Height; y++)
            {
                for (int x = 0; x < grid.Width; x++)
                {
                    if (grid.GetValue(x, y) == ERoomTypes.Free) continue;
                    var actualPosition = grid.GetWorldPosition(x, y);
                    var buildRoom = BuildRoom(new Vector2Int(x, y), grid.GetValue(x, y), new Vector2Int((int)actualPosition.x, (int)actualPosition.y), _yOffset);
                    roomObjects.Add(buildRoom);
                   // ColorRoom(buildRoom.GetComponent<Room>());
                }
            }
        }

        GameObject BuildRoom(Vector2Int _position, ERoomTypes _roomType, Vector2Int _placingPosition, int _yOffset = 0)
        {
            Vector3 rotation;
            ERoomDoorType chosenDoorType;

            Vector2Int[] directions = { Vector2Int.up, Vector2Int.right, Vector2Int.down, Vector2Int.left };
            bool[] hasDirection = { false, false, false, false };

            for (int i = 0; i < directions.Length; i++)
            {
                if (grid.GetValue(_position + directions[i]) != ERoomTypes.Free)
                {
                    hasDirection[i] = true;
                }
            }

            bool hasNorth = hasDirection[0];
            bool hasEast = hasDirection[1];
            bool hasSouth = hasDirection[2];
            bool hasWest = hasDirection[3];

            // switch (_doorAmount)
            // {
            //     case 1:
            //         chosenDoorType = ERoomDoorType.North;
            //         if (hasNorth) rotation = new Vector3(0, 0, 0);
            //         else if (hasEast) rotation = new Vector3(0, 90, 0);
            //         else if (hasSouth) rotation = new Vector3(0, 180, 0);
            //         else if (hasWest) rotation = new Vector3(0, -90, 0);
            //         break;
            //     case 2:
            //         if (hasNorth && hasSouth)
            //         {
            //             rotation = new Vector3(0, 0, 0);
            //             chosenDoorType = ERoomDoorType.NorthSouth;
            //         }
            //         else if (hasEast && hasWest)
            //         {
            //             rotation = new Vector3(0, 90, 0);
            //             chosenDoorType = ERoomDoorType.NorthSouth;
            //         }
            //         else if (hasNorth && hasEast)
            //         {
            //             rotation = new Vector3(0, 0, 0);
            //             chosenDoorType = ERoomDoorType.NorthEast;
            //         }
            //         else
            //         {
            //             rotation = new Vector3(0, -90, 0);
            //             chosenDoorType = ERoomDoorType.NorthEast;
            //         }
//
            //         break;
            //     case 3:
            //         chosenDoorType = ERoomDoorType.NorthEastWest;
            //         if (hasNorth && hasEast && hasWest) rotation = new Vector3(0, 0, 0);
            //         if (hasNorth && hasEast && hasSouth) rotation = new Vector3(0, 90, 0);
            //         if (hasEast && hasSouth && hasWest) rotation = new Vector3(0, 180, 0);
            //         if (hasNorth && hasSouth && hasWest) rotation = new Vector3(0, -90, 0);
            //         break;
            //     case 4:
            //         chosenDoorType = ERoomDoorType.Full;
            //         rotation = new Vector3(0, 0, 0);
            //         break;
            // }

            int doorAmount = GetNeighbourCount(_position);
            (rotation, chosenDoorType) = GetRotationAndDoorType(doorAmount, hasNorth, hasEast, hasSouth, hasWest);
            var correctRoomType = tileset.Rooms.Find(_room => _room.DoorType == chosenDoorType && _room.RoomType == _roomType).gameObject;
            if (correctRoomType == null) throw new ArgumentException("No valid room prefab found!");
            return Instantiate(correctRoomType, new Vector3(_placingPosition.x, -_yOffset, _placingPosition.y), Quaternion.Euler(rotation), groupParent);
        }

        (Vector3 rotation, ERoomDoorType doorType) GetRotationAndDoorType(int _doorAmount, bool _hasNorth, bool _hasEast, bool _hasSouth, bool _hasWest)
        {
            return _doorAmount switch
            {
                1 => GetSingleDoorRotation(_hasNorth, _hasEast, _hasSouth, _hasWest),
                2 => GetDoubleDoorRotation(_hasNorth, _hasEast, _hasSouth, _hasWest),
                3 => GetTripleDoorRotation(_hasNorth, _hasEast, _hasSouth, _hasWest),
                4 => (new Vector3(0, 0, 0), ERoomDoorType.Full),
                _ => throw new ArgumentException("Invalid door amount!")
            };
        }

        (Vector3 rotation, ERoomDoorType doorType) GetSingleDoorRotation(bool _hasNorth, bool _hasEast, bool _hasSouth, bool _hasWest)
        {
            if (_hasNorth) return (new Vector3(0, 0, 0), ERoomDoorType.North);
            if (_hasEast) return (new Vector3(0, 90, 0), ERoomDoorType.North);
            if (_hasSouth) return (new Vector3(0, 180, 0), ERoomDoorType.North);
            if (_hasWest) return (new Vector3(0, -90, 0), ERoomDoorType.North);
            throw new ArgumentException("Invalid door configuration!");
        }

        (Vector3 rotation, ERoomDoorType doorType) GetDoubleDoorRotation(bool _hasNorth, bool _hasEast, bool _hasSouth, bool _hasWest)
        {
            if (_hasNorth && _hasEast) return (new Vector3(0, 0, 0), ERoomDoorType.NorthEast);
            if (_hasNorth && _hasSouth) return (new Vector3(0, 0, 0), ERoomDoorType.NorthSouth);
            if (_hasNorth && _hasWest) return (new Vector3(0, -90, 0), ERoomDoorType.NorthEast);
            if (_hasEast && _hasWest) return (new Vector3(0, 90, 0), ERoomDoorType.NorthSouth);
            if (_hasEast && _hasSouth) return (new Vector3(0, 90, 0), ERoomDoorType.NorthEast);
            if (_hasSouth && _hasWest) return (new Vector3(0, 180, 0), ERoomDoorType.NorthEast);
            throw new ArgumentException("Invalid door configuration!");
        }

        (Vector3 rotation, ERoomDoorType doorType) GetTripleDoorRotation(bool _hasNorth, bool _hasEast, bool _hasSouth, bool _hasWest)
        {
            if (_hasNorth && _hasEast && _hasWest) return (new Vector3(0, 0, 0), ERoomDoorType.NorthEastWest);
            if (_hasNorth && _hasEast && _hasSouth) return (new Vector3(0, 90, 0), ERoomDoorType.NorthEastWest);
            if (_hasEast && _hasSouth && _hasWest) return (new Vector3(0, 180, 0), ERoomDoorType.NorthEastWest);
            if (_hasNorth && _hasSouth && _hasWest) return (new Vector3(0, -90, 0), ERoomDoorType.NorthEastWest);
            throw new ArgumentException("Invalid door configuration!");
        }


        int GetNeighbourCount(Vector2Int _position)
        {
            var neighbourCoords = new Vector2Int[]
            {
                _position + Vector2Int.right,
                _position + Vector2Int.left,
                _position + Vector2Int.up,
                _position + Vector2Int.down,
            };

            return neighbourCoords.Count(_currentPos => grid.GetValue(_currentPos) != ERoomTypes.Free);
        }

        [Button]
        public void ResetDungeon()
        {
            if (roomObjects.Count == 0) return;
            foreach (var roomObject in roomObjects)
            {
                DestroyImmediate(roomObject.gameObject);
            }

            roomObjects.Clear();
        }

        /// <summary>
        /// For testing purpose. Will not be needed later
        /// </summary>
        void ColorRoom(Room _room)
        {
            var localRenderer = _room.gameObject.GetComponent<MeshRenderer>();
            localRenderer.sharedMaterial = new Material(localRenderer.sharedMaterial);
            if (_room.RoomType == ERoomTypes.Boss)
            {
                localRenderer.sharedMaterial.color = Color.red;
                return;
            }

            localRenderer.sharedMaterial.color = _room.DoorType switch
            {
                ERoomDoorType.North => Color.white,
                ERoomDoorType.NorthEast => Color.green,
                ERoomDoorType.NorthSouth => Color.yellow,
                ERoomDoorType.NorthEastWest => Color.magenta,
                ERoomDoorType.Full => Color.cyan,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}