using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

namespace KI
{
    public class WalkToPointState : AnimationState
    {
        readonly NavMeshAgent agent;
        readonly TargetComponent target;
        const float RecalculationDistance = 0.5f;
        static readonly int isWalking = Animator.StringToHash("isWalking");
        static readonly int currentVelocityX = Animator.StringToHash("CurrentVelocityX");
        static readonly int currentVelocityZ = Animator.StringToHash("CurrentVelocityZ");

        public WalkToPointState(NavMeshAgent _agent, TargetComponent _target, Animator _animator) : base(_animator)
        {
            agent = _agent;
            target = _target;
        }

        public override void StateEnter()
        {
            animator.SetBool(isWalking, true);
            agent.SetDestination(target.TargetPosition);
        }

        public override void StateExit()
        {
            animator.SetBool(isWalking, false);
            // Debug.Log("Exit Chase");
        }

        public override void Tick()
        {
            if (Vector3.Distance(agent.destination, target.TargetPosition) >= RecalculationDistance)
            {
                agent.SetDestination(target.TargetPosition);
            }

            animator.SetFloat(currentVelocityX, agent.velocity.x);
            animator.SetFloat(currentVelocityZ, agent.velocity.z);
        }
    }
}