using System;
using System.Collections;
using UnityEngine;

namespace Spawner
{
    public class AutoSpawner : MonoBehaviour
    {
        [SerializeField] float playerDetectionRange;
        [SerializeField] LayerMask playerLayer;
        bool didAutoSpawning;
        
        
        AgentSpawner spawner;

        void Awake()
        {
            spawner = GetComponent<AgentSpawner>();
            StartCoroutine(CheckArea());

        }

        IEnumerator CheckArea()
        {
            while (!didAutoSpawning)
            {
                // Debug.Log("Checking for Player...");
                var overlap = Physics.OverlapSphere(transform.position, playerDetectionRange, playerLayer);
                if (overlap.Length > 0)
                {
                    spawner.TriggerSpawning();
                    didAutoSpawning = true;
                    StopAllCoroutines();
                }
                
                yield return new WaitForSeconds(0.25f); 
            }
        }
        
        void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, playerDetectionRange);
        }
    }
}