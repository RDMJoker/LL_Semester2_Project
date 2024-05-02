using System;
using Cinemachine;
using CombatSystems;
using UnityEngine;
using UnityEngine.AI;

namespace KI
{
    [RequireComponent(typeof(NavMeshAgent), typeof(Animator), typeof(HealthSystem))]
    public abstract class Agent : MonoBehaviour, IHitable
    {
        [SerializeField] public float AttackRange;
        [SerializeField] public float AttackDamage;
        [SerializeField] [Min(0.01f)] public float AttackSpeed;
        [SerializeField] protected bool StateMachineDebugMode;
        HealthSystem HealthSystem;
        protected TargetComponent TargetComponent;
        protected NavMeshAgent NavMeshAgent;
        protected Animator Animator;


        protected float DistanceToTarget => Vector3.Distance(transform.position, TargetComponent.TargetPosition);


        protected virtual void Awake()
        {
            NavMeshAgent = GetComponent<NavMeshAgent>();
            Animator = GetComponent<Animator>();
            HealthSystem = GetComponent<HealthSystem>();
        }

        protected abstract bool FindTarget(float _radius);

        public virtual void TakeDamage(float _value, GameObject _hitter)
        {
            HealthSystem.ReduceCurrentHP(_value);
            if (HealthSystem.IsDead) return;
            Debug.Log($"Aua! Ich habe noch {HealthSystem.CurrentHP} von maximal {HealthSystem.MaxHP} Leben!");
        }

        public virtual void OnHit(Agent _attackingAgent)
        {
            TakeDamage(_attackingAgent.AttackDamage, gameObject);
        }

        public virtual void OnDeath()
        {
            Debug.Log("Ich bin tot :( ");
            Destroy(gameObject);
        }
    }
}