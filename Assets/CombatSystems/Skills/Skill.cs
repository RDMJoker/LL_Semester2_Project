using System;
using System.Collections.Generic;
using KI;
using UnityEngine;

namespace CombatSystems.Skills
{
    public class Skill : MonoBehaviour, ISkill, IHasDamageType
    {
        [SerializeField] SkillData skillData;
        public float BaseDamage => skillData.SkillBaseDamage;
        public float Cooldown => skillData.SkillCooldown;
        public EDamageType DamageType => skillData.DamageType;

        List<Agent> hits = new();
        Agent castingAgent;
        EffectCollider effectCollider;

        public void SetCastingAgent(Agent _castingAgent)
        {
            castingAgent = _castingAgent;
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
            if (!_hit.TryGetComponent(out Agent target) || hits.Contains(target) || target.IsDead) return;
            Debug.Log(_hit.name);
            hits.Add(target);

            target.OnHit(castingAgent, BaseDamage + castingAgent.SpellDamage, DamageType);
            if (target.IsDead || !target.isActiveAndEnabled) hits.Remove(target);
        }

        
    }
}