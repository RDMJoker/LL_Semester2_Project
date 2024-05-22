using System;
using KI;
using LL_Unity_Utils.Timers;
using UnityEngine;

namespace CombatSystems
{
    [RequireComponent(typeof(Rigidbody),typeof(BoxCollider))]
    public class Projectile : MonoBehaviour
    {
        [SerializeField] float aliveDuration;
        Timer destructionTimer;
        bool didDamage;
        RangedWeapon weapon;
        Rigidbody projectileRigidbody;


        void Awake()
        {
            GetComponent<GenericTimerScript>().OverrideDuration(aliveDuration);
            projectileRigidbody = GetComponent<Rigidbody>();
        }

        public void SetWeaponReference(RangedWeapon _shooterWeapon)
        {
            weapon = _shooterWeapon;
        }

        void OnCollisionEnter(Collision _collider)
        {
            if (didDamage) return;
            if (_collider.gameObject.TryGetComponent(out IHitable target))
            {
                didDamage = true;
                weapon.DoDamage(target);
                Destroy(gameObject);
            }
            else
            {
                projectileRigidbody.isKinematic = true;
                Destroy(this);
            }
        }
    }
}