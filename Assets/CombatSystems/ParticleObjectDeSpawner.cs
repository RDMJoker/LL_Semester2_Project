using System;
using UnityEngine;

namespace CombatSystems
{
    public class ParticleObjectDeSpawner : MonoBehaviour
    {
        ParticleSystem particle;

        void Awake()
        {
            particle = GetComponent<ParticleSystem>();
            
        }

        void FixedUpdate()
        {
            if(!particle.isEmitting) Destroy(gameObject);
        }
    }
}