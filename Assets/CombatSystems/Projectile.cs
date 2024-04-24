using System;
using KI;
using UnityEngine;

namespace CombatSystems
{
    [RequireComponent(typeof(Rigidbody), typeof(BoxCollider))]
    public class Projectile : MonoBehaviour
    {
        [SerializeField] float flightDuration;
        Timer destructionTimer;
        bool didDamage;
        RangedWeapon weapon;


        void Awake()
        {
            destructionTimer = new Timer(flightDuration);
            destructionTimer.StartTimer();
        }

        public void SetWeaponReference(RangedWeapon _shooterWeapon)
        {
            weapon = _shooterWeapon;
        }

        void FixedUpdate()
        {
            if (destructionTimer.CheckTimer()) Destroy(gameObject);
        }

        // void OnTriggerEnter(Collider _collider)
        // {
        //     if (didDamage) return;
        //     if (_collider.TryGetComponent(out IHitable target))
        //     {
        //         didDamage = true;
        //         weapon.DoDamage(target);
        //         Destroy(gameObject);
        //     }
        // }

        void OnCollisionEnter(Collision _collision)
        {
            if (didDamage) return;
            if (_collision.gameObject.TryGetComponent(out IHitable target))
            {
                didDamage = true;
                weapon.DoDamage(target);
                Destroy(gameObject);
            }
            else
            {
                
            }
        }
    }
}