using UnityEngine;
using UnityEngine.AI;

namespace KI
{
    public class PlayerIdleState : State
    {
        readonly NavMeshAgent agent;
        readonly ObstacleAvoidanceType avoidanceType;

        public PlayerIdleState(NavMeshAgent _agent) : base()
        {
            agent = _agent;
            avoidanceType = agent.obstacleAvoidanceType;
        }

        public override void StateEnter()
        {
           //Debug.Log("I enter idle");
           agent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
        }

        public override void StateExit()
        {
            agent.obstacleAvoidanceType = avoidanceType;
        }

        public override void Tick()
        {
            //Debug.Log("Me warte");
            agent.velocity = Vector3.zero;
        }
    }
}