using System;
using KI;
using UnityEngine;

namespace CombatSystems
{
    public class Sword : Weapon
    {
        Agent weaponHolder;

        void Awake()
        {
            weaponHolder = GetComponentInParent<Agent>();
        }

        void DoDamage(IHitable _target)
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