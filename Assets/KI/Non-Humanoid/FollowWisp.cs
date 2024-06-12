using System;
using LL_Unity_Utils.Misc;
using UnityEngine;
using UnityEngine.AI;

namespace KI.Non_Humanoid
{
    public class FollowWisp : WispAgent
    {
        [SerializeField] float followDistance;
        [SerializeField] Transform objectToFollow;
        NavMeshAgent agent;

        TargetComponent followTarget;

        void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            followTarget = new TargetComponent();
            followTarget.SetTarget(objectToFollow);
            agent.stoppingDistance = followDistance;
            agent.speed = flySpeed;

            State followState = new WispFollowState(followTarget,agent);

            stateMachine = new StateMachine(followState, gameObject, debugStateMachine);
        }

        void FixedUpdate()
        {
            stateMachine.CheckSwapState();
        }
    }
}