using System.Collections.Generic;
using UnityEngine;

namespace Generation.DungeonGeneration.DungeonGenerationScriptables
{
    [CreateAssetMenu(menuName = "Scriptables/Generation/GenerationTileset", fileName = "NewTileset")]
    public class GenerationTileset : ScriptableObject
    {
        public List<Room> Rooms = new();
    }
}