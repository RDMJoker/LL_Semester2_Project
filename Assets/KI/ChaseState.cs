using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

namespace KI
{
    public class ChaseState : State
    {
        readonly NavMeshAgent agent;
        readonly GameObject target;

        public ChaseState(NavMeshAgent _agent, GameObject _target) : base()
        {
            agent = _agent;
            target = _target;
        }

        public override void StateEnter()
        {
            // Debug.Log("Enter chase");
            agent.speed = 5f;
            agent.SetDestination(target.transform.position);
            //Debug.Log(Transitions.Count);
        }

        public override void StateExit()
        {
            // Debug.Log("Exit Chase");
        }

        public override void Tick()
        {
            //Debug.Log("Tick");
            agent.SetDestination(target.transform.position);
        }
    }
}