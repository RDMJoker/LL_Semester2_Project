using KI;

namespace CombatSystems
{
    public interface IHitable
    {
        public void TakeDamage(float _value);
        public void OnHit(Agent _attackingAgent);
        public void OnDeath();
    }
}