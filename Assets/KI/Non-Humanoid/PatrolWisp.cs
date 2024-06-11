using System;
using LL_Unity_Utils.Misc;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace KI.Non_Humanoid
{
    public class PatrolWisp : WispAgent
    {
        [SerializeField] float flyRange;
        [SerializeField] float flyPointDistanceThreshhold;
        
        Vector3 spawnPosition;
        NavMeshAgent agent;
        TargetComponent patrolPointTarget;

        void Awake()
        {
            patrolPointTarget = new TargetComponent();
            spawnPosition = transform.position;
            agent = GetComponent<NavMeshAgent>();
            agent.speed = flySpeed;

            State startState = new WispRandomMovementState(agent,SetRandomPoint,patrolPointTarget);

            stateMachine = new StateMachine(startState,gameObject,debugStateMachine);
        }
        
        void SetRandomPoint()
        {
            Vector3 randomPoint;
            do
            {
                var unitSphere = Random.insideUnitSphere * flyRange;
                randomPoint = new Vector3(unitSphere.x, 0, unitSphere.z);
                randomPoint += spawnPosition;
            } while (!NavMesh.SamplePosition(randomPoint, out _, agent.radius * 2, agent.areaMask) || Vector3.Distance(transform.position, randomPoint) < flyPointDistanceThreshhold);
            
            patrolPointTarget.SetPoint(randomPoint);
        }

        void FixedUpdate()
        {
            stateMachine.CheckSwapState();
        }
    }
}