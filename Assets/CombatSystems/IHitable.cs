using KI;
using UnityEngine;

namespace CombatSystems
{
    public interface IHitable
    {
        public void TakeDamage(float _value, GameObject _hitter);
        public void OnHit(Agent _attackingAgent, float _damage, EDamageType _damageType);
        public void OnHit(float _damage, EDamageType _damageType);
        public void OnDeath();
    }
}