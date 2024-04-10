using System;
using UnityEngine;
using UnityEngine.AI;

namespace KI
{
    public class PlayerAgent : Agent
    {
        StateMachine stateMachine;


        protected override void Awake()
        {
            base.Awake();
            var idleState = new PlayerIdleState(NavMeshAgent);
            var walkingState = new PlayerWalkingState();
            stateMachine = new StateMachine(idleState);

            var idleToWalk = new Transition(walkingState, () => NavMeshAgent.hasPath);
            var walkToIdle = new Transition(idleState, () => NavMeshAgent.hasPath == false);

            idleState.AddTransition(idleToWalk);
            walkingState.AddTransition(walkToIdle);
        }

        protected override bool FindTarget(float _radius)
        {
            throw new NotImplementedException();
        }

        void FixedUpdate()
        {
            stateMachine.CheckSwapState();
        }
    }
}