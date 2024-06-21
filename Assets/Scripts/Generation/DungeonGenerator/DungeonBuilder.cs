using System;
using System.Collections.Generic;
using DefaultNamespace.Enums;
using LL_Unity_Utils.Generic;
using UnityEngine;

namespace Generation.DungeonGenerator
{
    public class DungeonBuilder : MonoBehaviour
    {
        [SerializeField] GameObject prefab;

        List<GameObject> roomObjects;

        void Awake()
        {
            roomObjects = new List<GameObject>();
        }

        public void BuildDungeon(ObjectGrid<ERoomTypes> _levelGrid)
        {
            ResetDungeon();
            for (int y = 0; y < _levelGrid.Height; y++)
            {
                for (int x = 0; x < _levelGrid.Width; x++)
                {
                    if (_levelGrid.GetValue(x, y) == ERoomTypes.Normal) roomObjects.Add(Instantiate(prefab, new Vector3(x, 0, y), Quaternion.identity));
                    if (_levelGrid.GetValue(x, y) != ERoomTypes.Boss) continue;
                    var bossRoom = Instantiate(prefab, new Vector3(x, 0, y), Quaternion.identity);
                    bossRoom.TryGetComponent(out MeshRenderer bossSpriteRenderer);
                    bossSpriteRenderer.material = new Material(bossSpriteRenderer.material)
                    {
                        color = Color.red
                    };
                    roomObjects.Add(bossRoom);
                }
            }
        }

        void ResetDungeon()
        {
            if (roomObjects.Count == 0) return;
            foreach (var roomObject in roomObjects)
            {
                Destroy(roomObject.gameObject);
            }

            roomObjects.Clear();
        }
    }
}