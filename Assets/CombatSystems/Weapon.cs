using System;
using KI;
using UnityEngine;

namespace CombatSystems
{
    public abstract class Weapon : MonoBehaviour, IHasDamageType
    {
        [SerializeField] EDamageType damageType;
        protected Agent weaponHolder;
        public abstract void DoDamage(IHitable _target);
        public EDamageType DamageType => damageType;
        

        protected void Awake()
        {
            weaponHolder = GetComponentInParent<Agent>();
        }

    }
}