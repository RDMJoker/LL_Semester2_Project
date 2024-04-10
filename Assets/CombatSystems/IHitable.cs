namespace CombatSystems
{
    public interface IHitable
    {
        public void TakeDamage(float _value);
        public void OnHit();
        public void OnDeath();
    }
}