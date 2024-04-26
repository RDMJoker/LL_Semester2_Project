using System;
using UnityEngine;

namespace CombatSystems
{
    public class PhysicsRotationArc : MonoBehaviour
    {
        Rigidbody referenceRigidbody;

        [SerializeField] Vector3 axis;

        void Awake()
        {
            referenceRigidbody = GetComponent<Rigidbody>();
        }

        void FixedUpdate()
        {
            if (referenceRigidbody.velocity.magnitude > 0)
            {
                transform.forward = transform.InverseTransformDirection(axis) + referenceRigidbody.velocity;
            }
        }
    }
}