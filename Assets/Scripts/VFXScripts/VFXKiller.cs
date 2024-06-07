using System;
using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

namespace VFXScripts
{
    public class VFXKiller : MonoBehaviour
    {
        VisualEffect visualEffect;

        void Awake()
        {
            visualEffect = GetComponent<VisualEffect>();
        }

        void Start()
        {
            StartCoroutine(CheckAliveParticles());
        }

        IEnumerator CheckAliveParticles()
        {
            bool hasAliveParticles = true;
            while (hasAliveParticles)
            {
                yield return new WaitForSeconds(1);
                if (visualEffect.aliveParticleCount == 0) hasAliveParticles = false;
            }
            Destroy(gameObject);
        }
    }
}