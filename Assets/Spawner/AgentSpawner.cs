using System;
using System.Collections;
using System.Collections.Generic;
using KI;
using MobData;
using NaughtyAttributes;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Spawner
{
    public class AgentSpawner : MonoBehaviour
    {
        [SerializeField] Transform spawnPoint;
        [SerializeField] Vector3 spawnPosition;
        [SerializeField] bool useTransform;
        [SerializeField] float spawnAmount;
        [SerializeField] List<MobDataScriptable> mobData;
        
        [Header("AutoSpawn Functions")]
        [SerializeField] bool autoSpawn;

        [SerializeField] float playerDetectionRange;
        [SerializeField] LayerMask playerLayer;
        bool didAutoSpawning;

        void Awake()
        {
            if (autoSpawn) StartCoroutine(CheckArea());
        }

        [Button]
        public void TriggerSpawning()
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

        void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, playerDetectionRange);
        }

        IEnumerator CheckArea()
        {
            while (!didAutoSpawning)
            {
                Debug.Log("Checking for Player...");
                var overlap = Physics.OverlapSphere(transform.position, playerDetectionRange, playerLayer);
                if (overlap.Length > 0)
                {
                    TriggerSpawning();
                    didAutoSpawning = true;
                    StopAllCoroutines();
                }
                
                yield return new WaitForSeconds(0.25f); 
            }
        }
    }
}