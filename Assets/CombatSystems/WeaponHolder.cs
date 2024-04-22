using UnityEngine;

namespace CombatSystems
{
    public class WeaponHolder : MonoBehaviour
    {
        [SerializeField] Weapon weapon;
        MeshCollider hitBox;


        void Awake()
        {
            hitBox = weapon.gameObject.GetComponent<MeshCollider>();
        }

        public void EnableWeaponCollider()
        {
            hitBox.enabled = true;
        }

        public void DisableWeaponCollider()
        {
            hitBox.enabled = false;
        }

        public void UseWeapon()
        {
            switch (weapon)
            {
                case RangedWeapon rangedWeapon:
                    rangedWeapon.Shoot();
                    break;
                case MeleeWeapon meleeWeapon:
                    break;
            }
        }
    }
}