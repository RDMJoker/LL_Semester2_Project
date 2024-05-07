using System;
using UnityEngine;

namespace KI
{
    public class PlayerAgent : Agent
    {
        [SerializeField] public LayerMask enemyMask;
        [SerializeField] float castingSpeed;
        StateMachine stateMachine;
        public bool IsWalking;
        public bool IsCasting;

        static readonly int castingSpeedHash = Animator.StringToHash("CastSpeed");
        static readonly int isCasting = Animator.StringToHash("isCasting");

        protected override void Awake()
        {
            base.Awake();
            TargetComponent = new TargetComponent();
            var idleState = new PlayerIdleState(NavMeshAgent);
            var walkingState = new WalkToPointState(NavMeshAgent, TargetComponent, Animator);
            var castingState = new CastingState(Animator, this, TargetComponent);
            stateMachine = new StateMachine(idleState, gameObject, StateMachineDebugMode);

            var idleToWalk = new Transition(walkingState, () => IsWalking);
            var walkToIdle = new Transition(idleState, () =>
            {
                if (NavMeshAgent.hasPath) return false;
                IsWalking = false;
                return true;
            });
            var anyToCasting = new Transition(castingState, () =>
            {
                if (!IsCasting) return false;
                NavMeshAgent.ResetPath();
                return true;
            });
            var castingToIdle = new Transition(idleState, () => !IsCasting);
            var castingToWalking = new Transition(walkingState, () => !IsCasting && IsWalking);

            var stunTimer = new Timer(StunDuration);
            var stunnedState = new StunnedState(Animator, stunTimer, this);
            var stunnedToIdle = new Transition(idleState, () =>
            {
                if (!stunTimer.CheckTimer()) return false;
                isStunned = false;
                return true;
            });

            var anyToStunned = new Transition(stunnedState, () => isStunned);

            idleState.AddTransition(anyToStunned);
            idleState.AddTransition(anyToCasting);
            idleState.AddTransition(idleToWalk);

            walkingState.AddTransition(anyToStunned);
            walkingState.AddTransition(anyToCasting);
            walkingState.AddTransition(walkToIdle);

            castingState.AddTransition(anyToStunned);
            castingState.AddTransition(castingToWalking);
            castingState.AddTransition(castingToIdle);

            stunnedState.AddTransition(stunnedToIdle);
        }

        void OnEnable()
        {
            CastingBehaviour.OnCastingEnd += SetCastingDone;
        }

        void OnDisable()
        {
            CastingBehaviour.OnCastingEnd -= SetCastingDone;
        }

        protected override bool FindTarget(float _radius)
        {
            throw new NotImplementedException();
        }

        void FixedUpdate()
        {
            Animator.SetFloat(castingSpeedHash, castingSpeed);
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

        public void SetCastingDone()
        {
            IsCasting = false;
        }
    }
}