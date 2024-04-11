using System;
using UnityEngine;
using UnityEngine.Events;

namespace CombatSystems
{
    public class HealthSystem : MonoBehaviour
    {
        [SerializeField] float maxHP;
        [SerializeField] float currentHP;
        public float CurrentHP => currentHP;
        public float MaxHP => maxHP;

        bool isDead;

        public bool IsDead
        {
            get => isDead;
            private set
            {
                if (isDead == value) return;
                if (value != true) isDead = false;
                OnDeath.Invoke();
                isDead = true;
            }
        }

        public UnityEvent OnDeath;

        public void ReduceCurrentHP(float _value)
        {
            currentHP = Mathf.Max(0, currentHP - _value);
            if (currentHP == 0) IsDead = true;
        }

        public void IncreaseCurrentHP(float _value)
        {
            currentHP = Mathf.Min(maxHP, currentHP + _value);
        }

        public void ReduceMaxHP(float _value)
        {
            maxHP = Mathf.Max(0, maxHP - _value);
            if (currentHP == 0) IsDead = true;
        }

        public void IncreaseMaxHP(float _value)
        {
            maxHP += _value;
        }

        /*  Maybe this?
          public void Revive(){}
        */
    }
}