﻿using System;
using System.Collections;
using CombatSystems;
using LL_Unity_Utils.Misc;
using LL_Unity_Utils.Timers;
using UnityEngine;
using UnityEngine.Profiling;

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
        [SerializeField] protected float AggroDuration;
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
                    aggroTimer = new Timer(AggroDuration);
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

        public override void OnHit(Agent _attackingAgent, float _damage, EDamageType _damageType)
        {
            TargetComponent.SetTarget(_attackingAgent.transform);
            IsAggro = true;
            base.OnHit(_attackingAgent,_damage, _damageType);
        }
    }
}