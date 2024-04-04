using System;
using UnityEngine;
using UnityEngine.AI;

namespace KI
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class PlayerAgent : MonoBehaviour
    {
        NavMeshAgent agent;
        float moveSpeed;
        StateMachine stateMachine;


        void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            var idleState = new PlayerIdleState(agent);
            var walkingState = new PlayerWalkingState();
            stateMachine = new StateMachine(idleState);

            var idleToWalk = new Transition(walkingState, () => agent.hasPath);
            var walkToIdle = new Transition(idleState, () => agent.hasPath == false);

            idleState.AddTransition(idleToWalk);
            walkingState.AddTransition(walkToIdle);
        }

        void FixedUpdate()
        {
            stateMachine.CheckSwapState();
        }
    }
}