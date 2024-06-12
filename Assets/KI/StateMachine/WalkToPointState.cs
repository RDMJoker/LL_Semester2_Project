using LL_Unity_Utils.Misc;
using ProgramWideConstants;
using UnityEngine;
using UnityEngine.AI;

namespace KI
{
    public class WalkToPointState : AnimationState
    {
        readonly NavMeshAgent agent;
        readonly TargetComponent target;
        static readonly int isWalking = Animator.StringToHash("isWalking");

        public WalkToPointState(NavMeshAgent _agent, TargetComponent _target, Animator _animator) : base(_animator)
        {
            agent = _agent;
            target = _target;
        }

        public override void StateEnter()
        {
            animator.SetBool(isWalking, true);
            agent.transform.LookAt(target.TargetPosition);
            agent.SetDestination(target.TargetPosition);
        }

        public override void StateExit()
        {
            animator.SetBool(isWalking, false);
            // Debug.Log("Exit Chase");
        }

        public override void Tick()
        {
            if (Vector3.Distance(agent.destination, target.TargetPosition) >= Constants.AIRecalculationDistance)
            {
                if (agent.isActiveAndEnabled) agent.SetDestination(target.TargetPosition);
            }
        }
    }
}