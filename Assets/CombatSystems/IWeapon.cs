namespace CombatSystems
{
    public interface IWeapon
    {
        public float Damage { get; }

        private void DoDamage(IHitable _target)
        {
        }
    }
}