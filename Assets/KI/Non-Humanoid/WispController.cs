using System;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace KI.Non_Humanoid
{
    public enum EWispBehaviour
    {
        Patrol,
        Protection,
        Follow
    }

    [RequireComponent(typeof(NavMeshAgent))]
    public class WispController : MonoBehaviour
    {
        [Header("Behaviour")]
        [SerializeField] EWispBehaviour behaviour;

        [Header("PatrolBehaviourValues")]
        [SerializeField] float flyRange;
        [SerializeField] float flySpeed;
        [SerializeField] float flyPointDistanceThreshhold;

        [Header("ProtectionBehaviourValues")]
        [SerializeField] Transform objectToProtect;
        [SerializeField] float patrolCircleRadius;
        [SerializeField] float rotateSpeed;

        [Header("FollowBehaviourValues")]
        [SerializeField] Transform objectToFollow;
        [SerializeField] float followDistance;

        NavMeshAgent agent;
        Vector3 spawnPosition;

        void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            switch (behaviour)
            {
                case EWispBehaviour.Patrol:
                    spawnPosition = transform.position;
                    agent.speed = flySpeed;
                    break;
                case EWispBehaviour.Protection:
                    var spawn = Random.insideUnitCircle.normalized * patrolCircleRadius;
                    var realSpawn = objectToProtect.position + new Vector3(spawn.x, transform.position.y, spawn.y);
                    transform.position = realSpawn;
                    break;
                case EWispBehaviour.Follow:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        void FixedUpdate()
        {
            switch (behaviour)
            {
                case EWispBehaviour.Patrol:
                    if (Vector3.Distance(agent.destination, transform.position) <= 0.95f) MoveToRandomPoint();
                    break;
                case EWispBehaviour.Protection:
                    if (ProtectionTargetDead()) Enrage();
                    else MoveInCircle();
                    break;
                case EWispBehaviour.Follow:
                    Follow();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        void MoveInCircle()
        {
            transform.RotateAround(objectToProtect.position, Vector3.up, rotateSpeed * Time.fixedDeltaTime);
        }

        void Follow()
        {
            agent.stoppingDistance = followDistance; 
            agent.SetDestination(objectToFollow.position);
        }

        bool ProtectionTargetDead()
        {
            return objectToProtect == null || !objectToProtect.gameObject.activeInHierarchy;
        }

        void Enrage()
        {
            // Enrage stuff
        }

        void MoveToRandomPoint()
        {
            Vector3 randomPoint;
            do
            {
                var unitSphere = Random.insideUnitSphere * flyRange;
                randomPoint = new Vector3(unitSphere.x, 0, unitSphere.z);
                randomPoint += spawnPosition;
            } while (!NavMesh.SamplePosition(randomPoint, out _, agent.radius * 2, agent.areaMask) || Vector3.Distance(transform.position, randomPoint) < flyPointDistanceThreshhold);

            agent.SetDestination(randomPoint);
        }
    }
}