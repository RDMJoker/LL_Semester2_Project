using System.Linq;
using CombatSystems;
using CombatSystems.Skills;
using LL_Unity_Utils.Misc;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.VFX;
using VFXScripts;

namespace KI.Non_Humanoid
{
    [RequireComponent(typeof(Animator), typeof(HealthSystem))]
    public class ProtectWisp : WispAgent, IHitable
    {
        [SerializeField] Transform objectToProtect;
        [SerializeField] float protectionRotationSpeed;
        [SerializeField] float protectionCircleRadius;
        [SerializeField] float playerDetectionRadius;
        [SerializeField] LayerMask detectionMask;
        [SerializeField] ProjectileSkill attackSkill;
        [SerializeField] GameObject wispBody;
        TargetComponent protectionTarget;
        TargetComponent attackTarget;
        NavMeshAgent agent;
        Animator animator;
        HealthSystem healthSystem;
        BoxCollider hitCollider;
        bool attackDone = true;
        bool isDead => healthSystem.IsDead;

        [ColorUsage(true, true)] [SerializeField]
        Color rageColor;


        void Awake()
        {
            hitCollider = GetComponentInChildren<BoxCollider>();
            healthSystem = GetComponent<HealthSystem>();
            protectionTarget = new TargetComponent();
            protectionTarget.SetTarget(objectToProtect);
            attackTarget = new TargetComponent();
            agent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
            var hoverMovement = GetComponentInChildren<WispHoverMovement>();

            var spawn = Random.insideUnitCircle.normalized * protectionCircleRadius;
            var realSpawn = objectToProtect.position + new Vector3(spawn.x, transform.position.y, spawn.y);
            transform.position = realSpawn;

            State startState = new ProtectState(protectionTarget, transform, protectionRotationSpeed);
            State attackState = new WispAttackState(attackTarget, animator, attackSpeed);
            State moveToState = new WispMoveToState(agent, attackTarget);
            State deathState = new DeathState(animator);
            stateMachine = new StateMachine(startState, gameObject, debugStateMachine);

            var toAttack = new Transition(attackState, () =>
            {
                if (objectToProtect != null) return false;
                if (!GetPlayer()) return false;
                Enrage();
                hoverMovement.enabled = false;
                animator.enabled = true;
                transform.position = new Vector3(transform.position.x, realSpawn.y, transform.position.z);
                return true;
            });
            var toMovement = new Transition(moveToState, () =>
            {
                if (!(Vector3.Distance(transform.position, attackTarget.TargetPosition) > attackRange) || !attackDone) return false;
                hoverMovement.enabled = true;
                animator.enabled = false;
                return true;
            });
            var moveToAttack = new Transition(attackState, () =>
            {
                if (!(Vector3.Distance(transform.position, attackTarget.TargetPosition) < attackRange)) return false;
                hoverMovement.enabled = false;
                animator.enabled = true;
                agent.ResetPath();
                transform.position = new Vector3(transform.position.x, realSpawn.y, transform.position.z);
                return true;
            });

            var anyToDeath = new Transition(deathState, () => isDead);

            startState.AddTransition(anyToDeath);
            startState.AddTransition(toAttack);

            attackState.AddTransition(anyToDeath);
            attackState.AddTransition(toMovement);

            moveToState.AddTransition(anyToDeath);
            moveToState.AddTransition(moveToAttack);
        }

        bool GetPlayer()
        {
            var overlap = Physics.OverlapSphere(transform.position, playerDetectionRadius, detectionMask);
            if (overlap.Length > 0)
            {
                attackTarget.SetTarget(overlap[0].transform);
                return true;
            }

            return false;
        }

        void FixedUpdate()
        {
            stateMachine.CheckSwapState();
        }

        void Enrage()
        {
            var colorChanger = GetComponentInChildren<VFXColorChanger>();
            colorChanger.ChangeColor(rageColor);
            transform.LookAt(attackTarget.TargetPosition);
        }

        void ShootProjectile()
        {
            transform.LookAt(attackTarget.TargetPosition);
            attackDone = false;
            var skillInstance = Instantiate(attackSkill, wispBody.transform.position, Quaternion.identity);
            skillInstance.transform.LookAt(attackTarget.TargetPosition + Vector3.up);
        }

        void SetAttackDone()
        {
            attackDone = true;
        }


        public void TakeDamage(float _value, GameObject _hitter)
        {
            healthSystem.ReduceCurrentHP(_value);
        }

        public void OnHit(Agent _attackingAgent, float _damage, EDamageType _damageType)
        {
            TakeDamage(_damage, _attackingAgent.gameObject);
            Debug.Log("Au :(");
        }

        public void OnHit(float _damage, EDamageType _damageType)
        {
            throw new System.NotImplementedException();
        }

        public void OnDeath()
        {
            Debug.Log("Im dead :( ");
            animator.enabled = true;
            agent.enabled = false;
            hitCollider.enabled = false;
        }

        public void DisableVFX()
        {
            var vfxSystem = GetComponentInChildren<VisualEffect>();
            vfxSystem.Stop();
        }
    }
}