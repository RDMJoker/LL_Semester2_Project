using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Spawner
{
    public class AreaTrigger : MonoBehaviour
    {
        [SerializeField] float detectionRange;
        [SerializeField] LayerMask detectionLayer;
        [SerializeField] bool triggerOnlyOnce;

        [Header("Multiple Trigger Options")] [SerializeField]
        int triggerAmount;

        [SerializeField] float durationBetweenTriggers;

        int timesTriggered;
        public UnityEvent OnAreaEnter;

        void Awake()
        {
            StartCoroutine(CheckArea());
        }

        IEnumerator CheckArea()
        {
            bool doneChecking = false;
            while (!doneChecking)
            {
                var overlap = Physics.OverlapSphere(transform.position, detectionRange, detectionLayer);
                if (overlap.Length > 0)
                {
                    OnAreaEnter.Invoke();
                    timesTriggered++;
                    if (triggerOnlyOnce || timesTriggered >= triggerAmount)
                    {
                        doneChecking = true;
                        StopAllCoroutines();
                    }
                    else
                    {
                        yield return new WaitForSeconds(durationBetweenTriggers);
                    }
                }

                yield return new WaitForSeconds(0.25f);
            }
        }


        void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, detectionRange);
        }
    }
}