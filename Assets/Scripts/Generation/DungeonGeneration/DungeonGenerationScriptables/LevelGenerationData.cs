using UnityEngine;

namespace Generation.DungeonGeneration.DungeonGenerationScriptables
{
    [CreateAssetMenu(menuName = "Scriptables/Generation/LevelGenerationData", fileName = "NewGenerationData")]
    public class LevelGenerationData : ScriptableObject
    {
        public int GenerationGridWidth;
        public int GenerationGridHeight;
        public int MinRoomCount;
        public float RoomCountIncrement;
        public bool UseStaticSeed;
        public int Seed;
        public GenerationTileset Tileset;
    }
}