using System;
using System.Linq;
using CombatSystems;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

namespace KI
{
    public class PlayerAgent : Agent
    {
        [SerializeField] public LayerMask enemyMask;
        StateMachine stateMachine;


        protected override void Awake()
        {
            base.Awake();
            var idleState = new PlayerIdleState(NavMeshAgent);
            var walkingState = new PlayerWalkingState();
            stateMachine = new StateMachine(idleState, gameObject);

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