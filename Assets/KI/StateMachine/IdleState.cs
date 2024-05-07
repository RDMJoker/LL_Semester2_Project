using System;
using System.Buffers.Text;
using UnityEngine;
using UnityEngine.AI;

namespace KI
{
    public class IdleState : AnimationState
    {
        readonly Timer timer;
        readonly NavMeshAgent agent;
        public bool IsTimerFinished;
        static readonly int isIdle = Animator.StringToHash("isIdle");

        public IdleState(Timer _timer, NavMeshAgent _agent, Animator _animator) : base(_animator)
        {
            timer = _timer;
            agent = _agent;
        }

        public override void StateEnter()
        {
            animator.SetBool(isIdle, true);
            timer.StartTimer();
           // agent.ResetPath();
        }

        public override void StateExit()
        {
            animator.SetBool(isIdle, false);
            IsTimerFinished = false;
        }

        public override void Tick()
        {
            IsTimerFinished = timer.CheckTimer();
        }
    }
}