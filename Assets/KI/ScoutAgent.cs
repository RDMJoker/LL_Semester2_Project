using System;
using CombatSystems;
using LL_Unity_Utils.Misc;
using LL_Unity_Utils.Timers;
using Spawner;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace KI
{
    public class ScoutAgent : EnemyAgent
    {
        AgentSpawner reinforcementSpawner;
        IdleState idleState;
        StateMachine stateMachine;
        [SerializeField] float timeTillDestruction;

        bool calledReinforcement;

        protected override void Awake()
        {
            base.Awake();
            reinforcementSpawner = GameObject.Find("Spawner").GetComponent<AgentSpawner>();
            PatrolRadiusCenter = transform.position;
            TargetComponent = new TargetComponent();
            IdleTargetComponent = new TargetComponent();
            var idleTimer = new Timer(IdleDuration);
            var runAwayDestructionTimer = new Timer(timeTillDestruction);
            idleState = new IdleState(idleTimer, NavMeshAgent, Animator);
            State patrolState = new PatrolState(NavMeshAgent, IdleTargetComponent, Animator, RecalculatePatrolPoint);
            State runToSpawnerState = new WalkToPointState(NavMeshAgent, TargetComponent, Animator);
            State runAwayState = new WalkToPointState(NavMeshAgent, TargetComponent, Animator);
            State summonReinforcement = new CallReinforcementState(Animator, TargetComponent, NavMeshAgent, runAwayDestructionTimer);
            State deathState = new DeathState(Animator);
            stateMachine = new StateMachine(idleState, gameObject, StateMachineDebugMode);


            var idleToPatrol = new Transition(patrolState, () => idleState.IsTimerFinished == true);
            var movingToIdle = new Transition(idleState, () => NavMeshAgent.remainingDistance < NavMeshAgent.stoppingDistance);
            var anyToRunToSpawner = new Transition(runToSpawnerState, () => FindTarget(SearchRadius) || IsAggro);
            var runToSpawnerToSummon = new Transition(summonReinforcement, () => Vector3.Distance(transform.position, TargetComponent.TargetPosition) <= NavMeshAgent.stoppingDistance);
            var summonToRunaway = new Transition(runAwayState, () => calledReinforcement);
            var destruction = new Transition(idleState, () =>
            {
                if (runAwayDestructionTimer.CheckTimer())
                {
                    Destroy(gameObject);
                }

                return false;
            });
            var anyToDeath = new Transition(deathState, () => IsDead);
            var deathToIdle = new Transition(idleState, () => !IsDead);

            
            idleState.AddTransition(anyToDeath);
            idleState.AddTransition(anyToRunToSpawner);
            idleState.AddTransition(idleToPatrol);

            patrolState.AddTransition(anyToDeath);
            patrolState.AddTransition(anyToRunToSpawner);
            patrolState.AddTransition(movingToIdle);

            runToSpawnerState.AddTransition(anyToDeath);
            runToSpawnerState.AddTransition(runToSpawnerToSummon);

            summonReinforcement.AddTransition(anyToDeath);
            summonReinforcement.AddTransition(summonToRunaway);

            runAwayState.AddTransition(anyToDeath);
            runAwayState.AddTransition(destruction);

            deathState.AddTransition(deathToIdle);
        }

        protected override bool FindTarget(float _radius)
        {
            var overlap = Physics.OverlapSphere(transform.position, SearchRadius, DetectionMask);
            if (overlap.Length > 0)
            {
                bool obstruction = Physics.Raycast(transform.position + (transform.up * 0.75f), (overlap[0].transform.position - transform.position).normalized, SearchRadius, DetectionObstructionMask);
                if (obstruction) return false;
                TargetComponent.SetTarget(reinforcementSpawner.transform);
                if (!IsAggro) IsAggro = true;

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

        public void SpawnReinforcements()
        {
            reinforcementSpawner.TriggerSpawning();
            calledReinforcement = true;
        }
        
        public override void OnHit(Agent _attackingAgent, float _damage, EDamageType _damageType)
        {
            IsAggro = true;
            IsStunned = HealthSystem.CheckStunned(_damage, StunThreshhold);
            TakeDamage(_damage, _attackingAgent.gameObject);
        }
    }
}