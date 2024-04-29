using System;
using UnityEngine;

namespace KI
{
    public abstract class EnemyAgent : Agent
    {
        protected TargetComponent IdleTargetComponent;
        [SerializeField] protected float IdleDuration;
        [SerializeField] protected float SearchRadius;
        [SerializeField] protected LayerMask DetectionMask;
        [SerializeField] protected LayerMask DetectionObstructionMask;
        [SerializeField] protected float PatrolRange;
        [SerializeField] protected float PatrolPointDistanceThreshhold;
        protected Vector3 PatrolRadiusCenter;
        protected bool AttackDone;
        protected bool IsAggro;

        void OnValidate()
        {
            PatrolPointDistanceThreshhold = Mathf.Clamp(PatrolPointDistanceThreshhold, 1, PatrolRange - 1);
        }

        public override void OnHit(Agent _attackingAgent)
        {
            TargetComponent.SetTarget(_attackingAgent.transform);
            IsAggro = true;
            base.OnHit(_attackingAgent);
        }
    }
}