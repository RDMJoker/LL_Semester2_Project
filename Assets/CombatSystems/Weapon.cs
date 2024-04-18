using UnityEngine;

namespace CombatSystems
{
    public abstract class Weapon : MonoBehaviour, IWeapon
    {
        public virtual float Damage { get; protected set; }
    }
}