using System;
using UnityEngine;

namespace CombatSystems
{
    public class PhysicsRotationArc : MonoBehaviour
    {
        Rigidbody referenceRigidbody;

        void Awake()
        {
            referenceRigidbody = GetComponent<Rigidbody>();
        }

        void FixedUpdate()
        {
            if (referenceRigidbody.velocity.magnitude > 0)
            {
                transform.forward = referenceRigidbody.velocity;
            }
        }
    }
}