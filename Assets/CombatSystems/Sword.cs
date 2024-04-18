using System;
using KI;
using UnityEngine;

namespace CombatSystems
{
    public class Sword : Weapon
    {
        [SerializeField] float damage;
        GameObject weaponHolder;
        


        public override float Damage
        {
            get => damage;
            protected set
            {
                if (Math.Abs(value - damage) < 0.01) return;
                damage = value;
            }
        }

        void Awake()
        {
            weaponHolder = GetComponentInParent<Agent>().gameObject;
        }

        void DoDamage(IHitable _target)
        {
            _target.TakeDamage(damage,weaponHolder);
        }

        void OnTriggerEnter(Collider _collider)
        {
            if (_collider.TryGetComponent(out IHitable target) && _collider.gameObject != weaponHolder)
            {
                DoDamage(target);
            }
        }
    }
}