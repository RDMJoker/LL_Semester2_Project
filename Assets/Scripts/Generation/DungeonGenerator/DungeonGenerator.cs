using Cinemachine;
using NaughtyAttributes;
using UnityEngine;

namespace Generation.DungeonGenerator
{
    public class DungeonGenerator : MonoBehaviour
    {
        [SerializeField] int levelAmount;
        
        
        [Button]
        void GenerateDungeon()
        {
            var levelGenerator = GetComponent<LevelGenerator>();
            for (int i = 1; i < levelAmount + 1; i++)
            {
                levelGenerator.ResetSeed();
                levelGenerator.InitMap(i);
            }
        }
    }
}