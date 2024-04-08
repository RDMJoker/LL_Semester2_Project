using System.Collections;
using TMPro;
using UnityEngine;

namespace KI
{
    public class Timer
    {
        float duration;
        float endTime;
        public Timer(float _duration)
        {
            duration = _duration;
        }

        public void StartTimer()
        {
            endTime = Time.time + duration;
        }

        public bool CheckTimer()
        {
            //return (Time.time - endTime) <= 0.01f;
            return Mathf.Approximately(Time.time, endTime);
        }
    }
}