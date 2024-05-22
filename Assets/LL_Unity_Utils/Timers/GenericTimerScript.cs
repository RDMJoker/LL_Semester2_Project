using UnityEngine;
using UnityEngine.Events;

namespace LL_Unity_Utils.Timers
{
    public class GenericTimerScript : MonoBehaviour
    {
        public float Duration;
        public UnityEvent OnTimerDone;
        public bool AutoStart;
        Timer timer;

        void Start()
        {
            timer ??= new Timer(Duration);
            if (AutoStart) timer.StartTimer();
        }

        void FixedUpdate()
        {
            if (timer.CheckTimer()) OnTimerDone.Invoke();
        }

        public void OverrideDuration(float _value)
        {
            Duration = _value;
            timer = new Timer(Duration);
            timer.StartTimer();
        }

        // List of basic functionality that might be useful

        public void DestroyGameObject()
        {
            Destroy(gameObject);
        }
    }
}