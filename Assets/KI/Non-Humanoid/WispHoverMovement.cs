using System;
using UnityEngine;
using UnityEngine.AI;

namespace KI.Non_Humanoid
{
    public class WispHoverMovement : MonoBehaviour
    {
        [SerializeField] float yMovementFrequence;
        [SerializeField] float yMovementAmplitude;

        [SerializeField] float timeOffset;
        [SerializeField] float yOffset;
        [SerializeField] float secondaryFrequence;
        [SerializeField] float secondaryAmplitude;

        float startY;

        void Start()
        {
            startY = transform.position.y;
        }


        void FixedUpdate()
        {
            var mainCurve = Mathf.Sin(Time.time * yMovementFrequence) * yMovementAmplitude;
            var secondaryCurve = Mathf.Sin((Time.time + timeOffset) * secondaryFrequence) * secondaryAmplitude + yOffset;
            transform.position = new Vector3(transform.position.x, startY + mainCurve + secondaryCurve, transform.position.z);
        }
    }
}