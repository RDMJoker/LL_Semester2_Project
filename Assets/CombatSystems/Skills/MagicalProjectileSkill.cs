using System;
using UnityEngine;

namespace CombatSystems.Skills
{
    public class MagicalProjectileSkill : ProjectileSkill, IHasDamageType
    {
        [SerializeField] EDamageType damageType;
        [SerializeField] float damage;
        public EDamageType DamageType => damageType;

        void OnParticleCollision(GameObject _hit)
        {
            if (!_hit.TryGetComponent(out IHitable target)) return;
            target.OnHit(damage, damageType);
        }
    }
}