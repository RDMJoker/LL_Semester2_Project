using System.Buffers.Text;
using UnityEngine;
using UnityEngine.AI;

namespace KI
{
    public class IdleState : State
    {
        float idleTime;
        NavMeshAgent agent;
        
        public IdleState(float _idleTime, NavMeshAgent _agent) : base()
        {
            idleTime = _idleTime;
            agent = _agent;
        }

        public override void StateEnter()
        {
            agent.speed = 0f;
            agent.ResetPath();
        }

        public override void StateExit()
        {
            base.StateExit();
        }

        public override void Tick()
        {
            //Debug.Log("waiting");
        }
    }
}