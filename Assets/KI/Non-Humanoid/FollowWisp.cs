using System;
using UnityEngine;
using UnityEngine.AI;

namespace KI.Non_Humanoid
{
    public class FollowWisp : WispAgent
    {
        [SerializeField] float followDistance;
        [SerializeField] Transform objectToFollow;
        NavMeshAgent agent;

        void Awake()
        {
            agent.stoppingDistance = followDistance;
        }

        void FixedUpdate()
        {
            agent.SetDestination(objectToFollow.position);
        }
    }
}