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
    }
}