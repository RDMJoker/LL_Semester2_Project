using UnityEngine;

namespace CombatSystems.Skills
{
    [CreateAssetMenu(menuName = "Scriptables/Skills/SkillData", fileName = "NewSkillData")]
    public class SkillData : ScriptableObject
    {
        public float SkillBaseDamage;
        public float SkillCooldown;
        //public EDamageType damageType;

    }
}