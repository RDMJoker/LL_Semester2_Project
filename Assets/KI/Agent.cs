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
        [SerializeField] protected float AttackRange;
        protected TargetComponent TargetComponent;
        protected NavMeshAgent NavMeshAgent;
        protected Animator Animator;
        protected HealthSystem HealthSystem;
        protected float DistanceToTarget => Vector3.Distance(transform.position, TargetComponent.TargetPosition) - NavMeshAgent.stoppingDistance;
        
        protected virtual void Awake()
        {
            NavMeshAgent = GetComponent<NavMeshAgent>();
            Animator = GetComponent<Animator>();
            HealthSystem = GetComponent<HealthSystem>();
        }

        protected abstract bool FindTarget(float _radius);

        public virtual void TakeDamage(float _value)
        {
           // HealthSystem.ReduceHp(_value);
        }

        public virtual void OnHit()
        {
            throw new System.NotImplementedException();
        }

        public virtual void OnDeath()
        {
            throw new System.NotImplementedException();
        }
    }
}