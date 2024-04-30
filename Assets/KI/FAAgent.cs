using System;
using Spawner;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace KI
{
    public class FAAgent : EnemyAgent
    {
        AgentSpawner reinforcementSpawner;
        IdleState idleState;
        StateMachine stateMachine;

        protected override void Awake()
        {
            base.Awake();
            reinforcementSpawner = GameObject.Find("Spawner").GetComponent<AgentSpawner>();
            PatrolRadiusCenter = transform.position;
            TargetComponent = new TargetComponent();
            IdleTargetComponent = new TargetComponent();
            var idleTimer = new Timer(IdleDuration);
            var runAwayDestructionTimer = new Timer(10);
            idleState = new IdleState(idleTimer, NavMeshAgent, Animator);
            State patrolState = new PatrolState(NavMeshAgent, IdleTargetComponent, Animator, RecalculatePatrolPoint);
            State runToSpawnerState = new WalkToPointState(NavMeshAgent, TargetComponent, Animator);
            State runAwayState = new WalkToPointState(NavMeshAgent, TargetComponent, Animator);
            State summonReinforcement = new CallReinforcementState(Animator, reinforcementSpawner, TargetComponent, NavMeshAgent, runAwayDestructionTimer);
            stateMachine = new StateMachine(idleState, gameObject);


            var idleToPatrol = new Transition(patrolState, () => idleState.IsTimerFinished == true);
            var movingToIdle = new Transition(idleState, () => NavMeshAgent.remainingDistance < NavMeshAgent.stoppingDistance);
            var anyToRunToSpawner = new Transition(runToSpawnerState, () => FindTarget(SearchRadius) || IsAggro);
            var runToSpawnerToSummon = new Transition(summonReinforcement, () => NavMeshAgent.remainingDistance <= NavMeshAgent.stoppingDistance);
            var summonToRunaway = new Transition(runAwayState, () => true);
            var destruction = new Transition(idleState, () =>
            {
                if (runAwayDestructionTimer.CheckTimer()) Destroy(gameObject);
                return false;
            });

            idleState.AddTransition(anyToRunToSpawner);
            idleState.AddTransition(idleToPatrol);

            patrolState.AddTransition(anyToRunToSpawner);
            patrolState.AddTransition(movingToIdle);

            runToSpawnerState.AddTransition(runToSpawnerToSummon);

            summonReinforcement.AddTransition(summonToRunaway);

            runAwayState.AddTransition(destruction);
        }

        protected override bool FindTarget(float _radius)
        {
            var overlap = Physics.OverlapSphere(transform.position, SearchRadius, DetectionMask);
            if (overlap.Length > 0)
            {
                if (!IsAggro) IsAggro = true;
                bool obstruction = Physics.Raycast(transform.position + (transform.up * 0.75f), (overlap[0].transform.position - transform.position).normalized, SearchRadius, DetectionObstructionMask);
                if (obstruction) return false;


                TargetComponent.SetTarget(reinforcementSpawner.transform);

                return true;
            }

            return false;
        }

        void RecalculatePatrolPoint()
        {
            Vector3 randomPoint;
            do
            {
                var unitSphere = Random.insideUnitSphere * PatrolRange;
                randomPoint = new Vector3(unitSphere.x, 0, unitSphere.z);
                randomPoint += PatrolRadiusCenter;
            } while (!NavMesh.SamplePosition(randomPoint, out _, NavMeshAgent.radius * 2, NavMeshAgent.areaMask) || Vector3.Distance(transform.position, randomPoint) < PatrolPointDistanceThreshhold);

            IdleTargetComponent.SetPoint(randomPoint);
        }

        void FixedUpdate()
        {
            stateMachine.CheckSwapState();
        }

        void OnDrawGizmos()
        {
            Gizmos.color = Color.black;
            Gizmos.DrawWireSphere(transform.position, SearchRadius);
        }
    }
}