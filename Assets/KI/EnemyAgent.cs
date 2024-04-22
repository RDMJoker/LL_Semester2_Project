using System;
using UnityEngine;

namespace KI
{
    public abstract class EnemyAgent : Agent
    {
        protected TargetComponent IdleTargetComponent;
        [SerializeField] protected float IdleDuration;
        [SerializeField] protected float SearchRadius;
        [SerializeField] protected LayerMask LayerMask;
        [SerializeField] protected float PatrolRange;
        [SerializeField] protected float PatrolPointDistanceThreshhold;
        protected Vector3 PatrolRadiusCenter;
        protected bool AttackDone;
        protected bool IsAggro;

        void OnValidate()
        {
            PatrolPointDistanceThreshhold = Mathf.Clamp(PatrolPointDistanceThreshhold, 1, PatrolRange);
        }

        public override void OnHit(Agent _attackingAgent)
        {
            base.OnHit(_attackingAgent);
            IsAggro = true;
        }
    }
    
    
}