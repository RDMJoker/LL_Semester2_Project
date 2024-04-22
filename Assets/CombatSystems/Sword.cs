using System;
using KI;
using UnityEngine;

namespace CombatSystems
{
    public class Sword : MeleeWeapon
    {
        void Awake()
        {
            weaponHolder = GetComponentInParent<Agent>();
        }

        public override void DoDamage(IHitable _target)
        {
            _target.TakeDamage(weaponHolder.AttackDamage, weaponHolder.gameObject);
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