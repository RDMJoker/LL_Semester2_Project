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
            var projectile = Instantiate(ammunitionPrefab, weaponHolder.transform.position, Quaternion.identity);
            projectile.GetComponent<Rigidbody>().AddForce(transform.forward * projectileSpeed, ForceMode.Impulse);
            projectile.SetWeaponReference(this);
        }
    }
}