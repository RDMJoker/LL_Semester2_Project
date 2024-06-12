using LL_Unity_Utils.Misc;
using ProgramWideConstants;
using UnityEngine;
using UnityEngine.AI;

namespace KI.Non_Humanoid
{
    public class WispFollowState : State
    {
        readonly TargetComponent target;
        readonly NavMeshAgent agent;

        public WispFollowState(TargetComponent _target, NavMeshAgent _agent)
        {
            target = _target;
            agent = _agent;
        }

        public override void StateEnter()
        {
            agent.SetDestination(target.TargetPosition);
        }

        public override void Tick()
        {
            if (Vector3.Distance(agent.destination, target.TargetPosition) >= Constants.AIRecalculationDistance)
            {
                agent.SetDestination(target.TargetPosition);
            }
        }
    }
}