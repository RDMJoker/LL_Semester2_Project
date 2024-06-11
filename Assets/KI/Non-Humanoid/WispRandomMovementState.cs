using System;
using LL_Unity_Utils.Misc;
using UnityEngine;
using UnityEngine.AI;

namespace KI.Non_Humanoid
{
    public class WispRandomMovementState : WispMoveToState
    {
        readonly Action setRandomPoint;

        const float RecalculationDistance = 0.5f;

        public WispRandomMovementState(NavMeshAgent _agent, Action _setRandomPoint, TargetComponent _patrolPointTarget) : base(_agent, _patrolPointTarget)
        {
            setRandomPoint = _setRandomPoint;
        }


        public override void StateEnter()
        {
            setRandomPoint();
            base.StateEnter();
        }

        public override void Tick()
        {
            if (Vector3.Distance(agent.transform.position, agent.destination) <= RecalculationDistance)
            {
                setRandomPoint();
            }

            base.Tick();
        }
    }
}