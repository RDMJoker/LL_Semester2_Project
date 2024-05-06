﻿using System;
using System.Collections.Generic;
using KI;
using UnityEngine;

namespace CombatSystems.Skills
{
    public class Skill : MonoBehaviour, ISkill
    {
        [SerializeField] SkillData skillData;
        public float BaseDamage => skillData.SkillBaseDamage;
        public float Cooldown => skillData.SkillCooldown;

        List<Agent> hits = new ();
        Agent castingAgent;
        ParticleSystem particle;
        EffectCollider effectCollider;

        public void SetCastingAgent(Agent _castingAgent)
        {
            castingAgent = _castingAgent;
            particle = GetComponent<ParticleSystem>();
            
        }

        void Awake()
        {
            effectCollider = GetComponentInChildren<EffectCollider>();
        }

        void Start()
        {
            
            transform.SetParent(null);
        }

        void OnEnable()
        {
            effectCollider.OnCollision += OnGameObjectHit;
        }

        void OnGameObjectHit(GameObject _hit)
        {
            if (!_hit.TryGetComponent(out Agent target) || hits.Contains(target)) return;
            Debug.Log(_hit.name);
            hits.Add(target);
            
            target.OnHit(castingAgent,BaseDamage + castingAgent.SpellDamage);
            if (target.IsDead || !target.isActiveAndEnabled) hits.Remove(target);
        }

        void FixedUpdate()
        {
            if(!particle.isEmitting) Destroy(gameObject);
        }
    }
}