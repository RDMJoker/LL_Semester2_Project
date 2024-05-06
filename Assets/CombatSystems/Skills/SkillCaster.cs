using System;
using KI;
using UnityEngine;

namespace CombatSystems.Skills
{
    public class SkillCaster : MonoBehaviour
    {
        [SerializeField] Skill skill;
        Agent castingAgent;
        Timer cooldownTimer;

        void Awake()
        {
            castingAgent = GetComponent<Agent>();
        }

        void OnEnable()
        {
            PlayerInputs.OnSkillButtonPressed += OnCastSkill;
        }

        void OnDisable()
        {
            PlayerInputs.OnSkillButtonPressed -= OnCastSkill;
        }

        void OnCastSkill()
        {
            if (cooldownTimer == null || cooldownTimer.CheckTimer())
            {
                var skillInstance = Instantiate(skill,transform);
                skillInstance.SetCastingAgent(castingAgent);
                cooldownTimer = new Timer(skillInstance.Cooldown);
                cooldownTimer.StartTimer();
            }
            else
            {
                Debug.Log("Still on Cooldown!");
            }
        }
    }
}