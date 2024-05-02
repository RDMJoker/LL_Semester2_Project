using System;
using UnityEngine;

namespace KI
{
    public class PlayerAgent : Agent
    {
        [SerializeField] public LayerMask enemyMask;
        StateMachine stateMachine;
        public bool IsWalking;


        protected override void Awake()
        {
            base.Awake();
            TargetComponent = new TargetComponent();
            var idleState = new PlayerIdleState(NavMeshAgent);
            var walkingState = new WalkToPointState(NavMeshAgent, TargetComponent, Animator);
            stateMachine = new StateMachine(idleState, gameObject,StateMachineDebugMode);

            var idleToWalk = new Transition(walkingState, () => IsWalking);
            var walkToIdle = new Transition(idleState, () =>
            {
                if (NavMeshAgent.hasPath) return false;
                IsWalking = false;
                return true;
            });

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

        public void SetTargetComponentPosition(Vector3 _position)
        {
            TargetComponent.SetPoint(_position);
        }

        void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position + Vector3.up, AttackRange);

        }
    }
}