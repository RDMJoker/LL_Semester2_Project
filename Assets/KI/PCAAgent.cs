using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace KI
{
    public class PCAAgent : EnemyAgent
    {
        StateMachine stateMachine;
        IdleState idleState;

        protected override void Awake()
        {
            base.Awake();
            PatrolRadiusCenter = transform.position;
            TargetComponent = new TargetComponent();
            IdleTargetComponent = new TargetComponent();
            var idleTimer = new Timer(IdleDuration);
            idleState = new IdleState(idleTimer, NavMeshAgent, Animator);
            State chaseState = new WalkToPointState(NavMeshAgent, TargetComponent, Animator);
            State returnToPointState = new WalkToPointState(NavMeshAgent, IdleTargetComponent, Animator);
            State patrolState = new PatrolState(NavMeshAgent, IdleTargetComponent, Animator, RecalculatePatrolPoint);
            State attackState = new AttackState(Animator, this);
            // State rotateToPlayerState = new RotateToPlayerState(Animator, TargetComponent, NavMeshAgent);
            stateMachine = new StateMachine(idleState, gameObject,StateMachineDebugMode);

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

            idleState.AddTransition(anyToChase);
            idleState.AddTransition(idleToPatrol);

            chaseState.AddTransition(chaseToReturn);
            chaseState.AddTransition(toAttack);

            returnToPointState.AddTransition(movingToIdle);
            returnToPointState.AddTransition(anyToChase);

            patrolState.AddTransition(movingToIdle);
            patrolState.AddTransition(anyToChase);

            attackState.AddTransition(attackToReturn);
            attackState.AddTransition(attackToChase);

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
                if(!IsAggro) IsAggro = true;
                bool obstruction = Physics.Raycast(transform.position + (transform.up * 0.75f), (overlap[0].transform.position - transform.position).normalized, SearchRadius, DetectionObstructionMask);
                if (obstruction) return false;
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