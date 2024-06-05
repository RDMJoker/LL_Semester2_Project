using System;
using UnityEngine;
using UnityEngine.AI;

namespace KI.Non_Humanoid
{
    public class WispHoverMovement : MonoBehaviour
    {
        [SerializeField] float yMovementFrequence;
        [SerializeField] float yMovementAmplitude;

        float startY;

        void Awake()
        {
            startY = transform.position.y;
        }


        void FixedUpdate()
        {
            transform.position = new Vector3(transform.position.x, startY + Mathf.Sin(Time.time * yMovementFrequence) * yMovementAmplitude, transform.position.z);
        }
    }
}