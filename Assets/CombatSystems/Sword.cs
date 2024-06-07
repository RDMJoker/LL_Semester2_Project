using System;
using KI;
using UnityEngine;

namespace CombatSystems
{
    public class Sword : MeleeWeapon
    {
        public override void DoDamage(IHitable _target)
        {
            _target.OnHit(weaponHolder, weaponHolder.AttackDamage, DamageType);
        }

        void OnTriggerEnter(Collider _collider)
        {
            if (_collider.TryGetComponent(out IHitable target))
            {
                DoDamage(target);
            }
        }
    }
}