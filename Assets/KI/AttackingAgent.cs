using CombatSystems;
using LL_Unity_Utils.Misc;
using LL_Unity_Utils.Timers;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace KI
{
    public class AttackingAgent : EnemyAgent
    {
        StateMachine stateMachine;
        IdleState idleState;

        [Header("Simulation Scene Options - ONLY FOR SHOWCASE")]
        [SerializeField] bool isStationary;

        protected override void Awake()
        {
            base.Awake();
            PatrolRadiusCenter = transform.position;
            TargetComponent = new TargetComponent();
            IdleTargetComponent = new TargetComponent();
            if (isStationary) IdleTargetComponent.SetPoint(transform.position);
            var idleTimer = new Timer(IdleDuration);
            idleState = new IdleState(idleTimer, NavMeshAgent, Animator);
            State chaseState = new WalkToPointState(NavMeshAgent, TargetComponent, Animator);
            State returnToPointState = new WalkToPointState(NavMeshAgent, IdleTargetComponent, Animator);
            State patrolState = new PatrolState(NavMeshAgent, IdleTargetComponent, Animator, RecalculatePatrolPoint);
            State attackState = new AttackState(Animator, this);
            State deathState = new DeathState(Animator);
            // State rotateToPlayerState = new RotateToPlayerState(Animator, TargetComponent, NavMeshAgent);
            stateMachine = new StateMachine(idleState, gameObject, StateMachineDebugMode);

            var anyToChase = new Transition(chaseState, () => FindTarget(SearchRadius) || IsAggro);
            var idleToPatrol = new Transition(patrolState, () => idleState.IsTimerFinished == true);
            var movingToIdle = new Transition(idleState, () => NavMeshAgent.remainingDistance < NavMeshAgent.stoppingDistance);
            var chaseToReturn = new Transition(returnToPointState, () => FindTarget(SearchRadius) == false && !IsAggro);
            var toAttack = new Transition(attackState, () =>
            {
                if (DistanceToTarget >= AttackRange) return false;
                NavMeshAgent.ResetPath();
                AttackDone = false;
                return true;
            });
            var attackToChase = new Transition(chaseState, () => DistanceToTarget >= AttackRange && AttackDone);
            var attackToReturn = new Transition(returnToPointState, () => FindTarget(SearchRadius) == false && AttackDone && !IsAggro);
            var anyToDeath = new Transition(deathState, () => IsDead);
            var deathToIdle = new Transition(idleState, () => !IsDead);
            var stunTimer = new Timer(StunDuration);
            var stunnedState = new StunnedState(Animator, stunTimer, this);

            var anyToStunned = new Transition(stunnedState, () =>
            {
                if (!isStunned) return false;
                AttackDone = true;
                return true;
            });
            var stunnedToIdle = new Transition(idleState, () =>
            {
                if (!stunTimer.CheckTimer()) return false;
                isStunned = false;
                return true;
            });


            idleState.AddTransition(anyToDeath);
            idleState.AddTransition(anyToStunned);
            idleState.AddTransition(anyToChase);
            if (!isStationary) idleState.AddTransition(idleToPatrol);

            chaseState.AddTransition(anyToDeath);
            chaseState.AddTransition(anyToStunned);
            chaseState.AddTransition(chaseToReturn);
            chaseState.AddTransition(toAttack);

            returnToPointState.AddTransition(anyToDeath);
            returnToPointState.AddTransition(anyToStunned);
            returnToPointState.AddTransition(movingToIdle);
            returnToPointState.AddTransition(anyToChase);

            if (!isStationary)
            {
                patrolState.AddTransition(anyToDeath);
                patrolState.AddTransition(anyToStunned);
                patrolState.AddTransition(movingToIdle);
                patrolState.AddTransition(anyToChase);
            }

            attackState.AddTransition(anyToDeath);
            attackState.AddTransition(anyToStunned);
            attackState.AddTransition(attackToReturn);
            attackState.AddTransition(attackToChase);

            stunnedState.AddTransition(anyToDeath);
            stunnedState.AddTransition(stunnedToIdle);

            deathState.AddTransition(deathToIdle);
        }


        void FixedUpdate()
        {
            CheckAggroState();
            stateMachine.CheckSwapState();
        }

        protected override bool FindTarget(float _radius)
        {
            var overlap = Physics.OverlapSphere(transform.position, SearchRadius, DetectionMask);
            if (overlap.Length > 0)
            {
                bool obstruction = Physics.Raycast(transform.position + (transform.up * 0.75f), (overlap[0].transform.position - transform.position).normalized, SearchRadius, DetectionObstructionMask);
                if (obstruction) return false;
                if (!IsAggro) IsAggro = true;
                TargetComponent.SetTarget(overlap[0].transform);
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

        void OnDrawGizmos()
        {
            Gizmos.color = Color.black;
            Gizmos.DrawWireSphere(transform.position, SearchRadius);
            // Gizmos.color = Color.yellow;
            // Gizmos.DrawSphere(transform.position + Vector3.up, AttackRange);
        }


        void AttackStart()
        {
            transform.LookAt(new Vector3(TargetComponent.TargetPosition.x, transform.position.y, TargetComponent.TargetPosition.z), Vector3.up);
            AttackDone = false;
        }

        void SetAttackDone()
        {
            AttackDone = true;
        }
    }
}