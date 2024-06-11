using LL_Unity_Utils.Misc;
using UnityEngine.AI;

namespace KI.Non_Humanoid
{
    public class WispMoveToState : State
    {
       protected readonly NavMeshAgent agent;
       readonly TargetComponent patrolPointTarget;
        public WispMoveToState(NavMeshAgent _agent, TargetComponent _patrolPointTarget)
        {
            agent = _agent;
            patrolPointTarget = _patrolPointTarget;
        }

        public override void StateEnter()
        {
            agent.SetDestination(patrolPointTarget.TargetPosition);
        }

        public override void Tick()
        {
            agent.SetDestination(patrolPointTarget.TargetPosition);
        }
    }
}