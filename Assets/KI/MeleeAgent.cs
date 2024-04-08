using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

namespace KI
{
    [RequireComponent(typeof(NavMeshAgent), typeof(Animator))]
    public class MeleeAgent : MonoBehaviour
    {
        TargetComponent targetComponent;
        TargetComponent idleTargetComponent;
        [SerializeField] LayerMask layerMask;
        [SerializeField] float searchRadius;
        [SerializeField] float distanceThreshhold;
        [SerializeField] float attackRange;
        Vector3 patrolRadiusCenter;
        Animator animator;
        StateMachine stateMachine;
        NavMeshAgent navMeshAgent;
        float moveSpeed;
        IdleState startState;
        bool attackDone;
        
        float DistanceToEnemy => Vector3.Distance(transform.position, targetComponent.TargetPosition) - navMeshAgent.stoppingDistance;

        void Awake()
        {
            patrolRadiusCenter = transform.position;
            targetComponent = new TargetComponent();
            idleTargetComponent = new TargetComponent();
            navMeshAgent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
            var idleTimer = new Timer(1f);
            startState = new IdleState(idleTimer, navMeshAgent, animator);
            State chaseState = new WalkToPointState(navMeshAgent, targetComponent, animator);
            State returnToPointState = new WalkToPointState(navMeshAgent, idleTargetComponent, animator);
            State patrolState = new PatrolState(navMeshAgent, idleTargetComponent, animator, RecalculatePatrolPoint);
            State attackState = new AttackState(animator);
            stateMachine = new StateMachine(startState);

            var anyToChase = new Transition(chaseState, () => FindPlayer(searchRadius));
            var idleToPatrol = new Transition(patrolState, () => startState.IsTimerFinished == true);
            var movingToIdle = new Transition(startState, () => navMeshAgent.remainingDistance < navMeshAgent.stoppingDistance);
            var anyToReturn = new Transition(returnToPointState, () => FindPlayer(searchRadius) == false && attackDone);
            var toAttack = new Transition(attackState, () =>
            {
                if (!(DistanceToEnemy <= attackRange)) return false;
                attackDone = false;
                return true;
            });
            var attackToChase = new Transition(chaseState, () => DistanceToEnemy >= attackRange && attackDone);
            var attackToAttack = new Transition(attackState, () => attackDone);

            startState.AddTransition(anyToChase);
            startState.AddTransition(idleToPatrol);
            
            chaseState.AddTransition(anyToReturn);
            chaseState.AddTransition(toAttack);
            
            returnToPointState.AddTransition(movingToIdle);
            returnToPointState.AddTransition(anyToChase);
            
            patrolState.AddTransition(movingToIdle);
            patrolState.AddTransition(anyToChase);
            
            attackState.AddTransition(anyToReturn);
            attackState.AddTransition(attackToChase);
            attackState.AddTransition(attackToAttack);
        }

        

        void FixedUpdate()
        {
            stateMachine.CheckSwapState();
        }

        bool FindPlayer(float _radius)
        {
            var overlap = Physics.OverlapSphere(this.transform.position, searchRadius, layerMask);
            if (overlap.Length > 0)
            {
                targetComponent.SetTarget(overlap[0].transform);
                
                return true;
            }

            return false;
        }

        void RecalculatePatrolPoint()
        {
            Vector3 randomPoint;
            do
            {
                var temp = Random.insideUnitSphere * 5f;
                randomPoint = new Vector3(temp.x, 0, temp.z);
                randomPoint += patrolRadiusCenter;
            } while (!NavMesh.SamplePosition(randomPoint, out _, navMeshAgent.radius * 2, navMeshAgent.areaMask) || Vector3.Distance(transform.position, randomPoint) < distanceThreshhold);

            idleTargetComponent.SetPoint(randomPoint);
        }

        void OnDrawGizmos()
        {
            Gizmos.color = Color.black;
            Gizmos.DrawSphere(transform.position, 5f);
        }

        void AttackDone()
        {
            attackDone = true;
        }

        void AttackStart()
        {
            attackDone = false;
        }
    }
}