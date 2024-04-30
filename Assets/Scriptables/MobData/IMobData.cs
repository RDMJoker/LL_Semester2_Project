using System.Numerics;
using KI;

namespace MobData
{
    public interface IMobData
    {
        public Agent AgentToSpawn { get; }
        public float RandomSpawnRange { get; }
    }
}