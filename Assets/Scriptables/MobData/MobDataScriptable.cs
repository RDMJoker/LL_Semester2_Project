using KI;
using UnityEngine;

namespace MobData
{
    [CreateAssetMenu(menuName = "Scriptables/MobDataScriptable", fileName = "DefaultSpawner")]
    public class MobDataScriptable : ScriptableObject, IMobData
    {
        [SerializeField] Agent agentToSpawn;
        [SerializeField] float randomSpawnRange;
        public Agent AgentToSpawn => agentToSpawn;
        public float RandomSpawnRange => randomSpawnRange;
    }
}