using System.Collections;
using TMPro;
using UnityEngine;

namespace KI
{
    public class Timer
    {
        readonly float duration;
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
            return Mathf.Approximately(Time.time, endTime);
        }
    }
}