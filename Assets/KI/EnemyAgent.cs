using System;
using System.Collections;
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
        [SerializeField] protected float aggroDuration;
        protected Vector3 PatrolRadiusCenter;
        protected bool AttackDone;
        bool isAggro;
        Timer aggroTimer;

        protected bool IsAggro
        {
            get => isAggro;
            set
            {
                // Debug.Log($"Current isAggro: {isAggro} -> Value IsAggro: {value}");
                if (value == isAggro) return;
                if (value)
                {
                    aggroTimer = new Timer(aggroDuration);
                    aggroTimer.StartTimer();
                }

                isAggro = value;
            }
        }

        protected void CheckAggroState()
        {
            if (aggroTimer != null) IsAggro = !aggroTimer.CheckTimer();
        }


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