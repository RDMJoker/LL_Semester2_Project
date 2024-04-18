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
        [SerializeField] protected float MoveSpeed;
        [SerializeField] public float AttackRange;
        [SerializeField] public float AttackDamage;
        HealthSystem HealthSystem;
        protected TargetComponent TargetComponent;
        protected NavMeshAgent NavMeshAgent;
        protected Animator Animator;
        protected bool IsAggro;

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
            TargetComponent.SetTarget(_attackingAgent.transform);
            TakeDamage(_attackingAgent.AttackDamage, gameObject);
            IsAggro = true;
        }

        public virtual void OnDeath()
        {
            Debug.Log("Ich bin tot :( ");
            Destroy(gameObject);
        }
    }
}