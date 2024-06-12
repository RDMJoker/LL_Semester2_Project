using System;
using UnityEngine;

namespace Program
{
    public class ResolutionForcer : MonoBehaviour
    {
        void Awake()
        {
            Screen.SetResolution(1920, 1080, true);
        }
    }
}