﻿using System;
using UnityEngine;

namespace CombatSystems
{
    public class Bow : RangedWeapon
    {
        [SerializeField] Projectile ammunitionPrefab;
        [SerializeField] float projectileSpeed;

        public override void DoDamage(IHitable _target)
        {
            _target.TakeDamage(weaponHolder.AttackDamage, weaponHolder.gameObject);
        }

        public override void Shoot()
        {
            var projectile = Instantiate(ammunitionPrefab, transform.position, Quaternion.LookRotation(weaponHolder.transform.forward));
            projectile.gameObject.GetComponent<Rigidbody>().AddForce(weaponHolder.transform.forward * projectileSpeed, ForceMode.Impulse);
            projectile.SetWeaponReference(this);
        }
    }
}