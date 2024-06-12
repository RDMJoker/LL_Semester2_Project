using System;
using UnityEngine;

namespace LoadingScreen
{
    public class WispLoadingCircle : MonoBehaviour
    {
        [SerializeField] GameObject objectToRotateAround;
        [SerializeField] float rotateSpeed;
        [SerializeField] Vector3 axis;

        void FixedUpdate()
        {
            Rotate();
        }

        void Rotate()
        {
            transform.RotateAround(objectToRotateAround.transform.position, axis, rotateSpeed * Time.fixedDeltaTime);
        }
    }
}