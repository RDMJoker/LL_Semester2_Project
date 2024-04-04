using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering.Universal;

namespace KI
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class MeleeAgent : MonoBehaviour
    {
        [SerializeField] GameObject player;
        StateMachine stateMachine;
        NavMeshAgent navMeshAgent;
        float moveSpeed;
        State startState;
        [SerializeField]LayerMask layerMask;
        

        void Awake()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            startState = new IdleState(0.5f, navMeshAgent);
            State chaseState = new ChaseState(navMeshAgent, player);
            stateMachine = new StateMachine(startState);

            var idleToChase = new Transition(chaseState,() => Physics.OverlapSphere(this.transform.position, 5f,layerMask).Length > 0);
            var chaseToIdle = new Transition(startState, () => Physics.OverlapSphere(this.transform.position, 5f, layerMask).Length == 0);
            
            startState.AddTransition(idleToChase);
            chaseState.AddTransition(chaseToIdle);
        }

        void FixedUpdate()
        {
            stateMachine.CheckSwapState();
        }

        void OnDrawGizmos()
        {
            Gizmos.color = Color.black;
            Gizmos.DrawSphere(transform.position, 5f);
        }
    }
}