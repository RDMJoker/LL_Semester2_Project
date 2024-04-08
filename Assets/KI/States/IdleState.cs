using System;
using System.Buffers.Text;
using UnityEngine;
using UnityEngine.AI;

namespace KI
{
    public class IdleState : AnimationState
    {
        Timer timer;
        NavMeshAgent agent;

        Action onIdleExit;

        public bool IsTimerFinished;
        public IdleState(Timer _timer, NavMeshAgent _agent, Animator _animator) : base(_animator)
        {
            timer = _timer;
            agent = _agent;
        }

        public override void StateEnter()
        {
            animator.SetBool(IsIdle, true);
            timer.StartTimer();
            agent.ResetPath();
        }

        public override void StateExit()
        {
            animator.SetBool(IsIdle, false);
            IsTimerFinished = false;
        }

        public override void Tick()
        {
            IsTimerFinished = timer.CheckTimer();
        }
    }
}