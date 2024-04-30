using System.Collections.Generic;
using KI;
using MobData;
using NaughtyAttributes;
using UnityEngine;

namespace Spawner
{
    public class AgentSpawner : MonoBehaviour
    {
        [SerializeField] Transform spawnPoint;
        [SerializeField] Vector3 spawnPosition;
        [SerializeField] bool useTransform;
        [SerializeField] float spawnAmount;
        [SerializeField] List<MobDataScriptable> mobData;


        [Button]
        void TriggerSpawning()
        {
            foreach (var mob in mobData)
            {
                for (int i = 0; i < spawnAmount; i++)
                {
                    switch (useTransform)
                    {
                        case true:
                            Spawn(spawnPoint.position, mob.RandomSpawnRange, mob.AgentToSpawn);
                            break;
                        case false:
                            Spawn(spawnPosition, mob.RandomSpawnRange, mob.AgentToSpawn);
                            break;
                    }
                }
            }
        }

        void Spawn(Vector3 _position, float _randomSpawnRange, Agent _agent)
        {
            var randomPos = Random.insideUnitSphere * _randomSpawnRange;
            var instantiatePos = new Vector3(randomPos.x, 0, randomPos.y);
            instantiatePos += _position;
            Instantiate(_agent, instantiatePos, Quaternion.identity);
        }
    }
}