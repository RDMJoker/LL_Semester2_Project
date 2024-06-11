using System;
using KI;
using LL_Unity_Utils.Misc;
using UnityEngine;

namespace KI.Non_Humanoid
{
    public class WispAgent : MonoBehaviour
    {
        [SerializeField] protected float attackRange;
        [SerializeField] protected float flySpeed;
        [SerializeField] protected float attackSpeed;
        [SerializeField] protected bool debugStateMachine;

        protected StateMachine stateMachine;
    }
}