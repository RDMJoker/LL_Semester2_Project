using UnityEngine;

namespace LL_Unity_Utils.Timers
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
            return Time.time > endTime;
        }
    }
}