using System;
using KI;
using LL_Unity_Utils.Timers;
using Unity.Mathematics;
using UnityEngine;

namespace CombatSystems.Skills
{
    public class SkillCaster : MonoBehaviour
    {
        [SerializeField] Skill skill;
        Agent castingAgent;
        Timer cooldownTimer;

        Vector3 savedMousePosition;

        void Awake()
        {
            castingAgent = GetComponent<Agent>();
        }

        void OnEnable()
        {
            PlayerInputs.OnSkillButtonPressed += SaveMousePosition;
        }

        void OnDisable()
        {
            PlayerInputs.OnSkillButtonPressed -= SaveMousePosition;
        }

        void SaveMousePosition(Vector3 _mousePosition)
        {
            savedMousePosition = _mousePosition;
        }

        public void OnCastSkill()
        {
            var skillInstance = Instantiate(skill, transform.position, transform.rotation);
            skillInstance.transform.LookAt(new Vector3(savedMousePosition.x, transform.position.y, savedMousePosition.z), Vector3.up);
            skillInstance.SetCastingAgent(castingAgent);
            cooldownTimer = new Timer(skillInstance.Cooldown);
            cooldownTimer.StartTimer();
        }

        public bool GetCurrentTimerStatus()
        {
            return cooldownTimer == null || cooldownTimer.CheckTimer();
        }
    }
}