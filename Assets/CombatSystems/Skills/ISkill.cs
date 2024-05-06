namespace CombatSystems.Skills
{
    public interface ISkill
    {
        public float BaseDamage { get; }
        public float Cooldown { get; }
    }
}