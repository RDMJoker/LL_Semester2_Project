using LL_Unity_Utils.Misc;
using UnityEngine;
using UnityEngine.AI;

namespace KI
{
    public class RotateToPlayerState : AnimationState
    {
        readonly TargetComponent target;
        readonly NavMeshAgent agent;
        static readonly int isIdle = Animator.StringToHash("isIdle");
        public RotateToPlayerState(Animator _animator, TargetComponent _target, NavMeshAgent _agent) : base(_animator)
        {
            agent = _agent;
            target = _target;
        }

        public override void StateEnter()
        {
            animator.SetBool(isIdle,true);
            agent.transform.LookAt(target.TargetPosition);
        }

        public override void Tick()
        {
            agent.transform.LookAt(target.TargetPosition);
        }
    }
}