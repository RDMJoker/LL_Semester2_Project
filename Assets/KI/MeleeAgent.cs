using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering.Universal;

namespace KI
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class MeleeAgent : MonoBehaviour
    {
        StateMachine stateMachine;
        List<Transition> transitions;
        NavMeshAgent navMeshAgent;
        float moveSpeed;
        State startState;

        void Awake()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            startState = new IdleState(0.5f);
            stateMachine = new StateMachine(startState);
        }
    }
}