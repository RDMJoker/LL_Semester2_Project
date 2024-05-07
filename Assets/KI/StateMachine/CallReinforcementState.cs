using Spawner;
using UnityEngine;
using UnityEngine.AI;

namespace KI
{
    public class CallReinforcementState : AnimationState
    {
        readonly AgentSpawner spawner;
        readonly TargetComponent targetComponent;
        readonly NavMeshAgent agent;
        readonly Timer runawayTimerReference;

        static readonly int isCalling = Animator.StringToHash("isCalling");
        
        public CallReinforcementState(Animator _animator, AgentSpawner _spawner, TargetComponent _targetComponent, NavMeshAgent _agent, Timer _runawayTimerReference) : base(_animator)
        {
            spawner = _spawner;
            targetComponent = _targetComponent;
            agent = _agent;
            runawayTimerReference = _runawayTimerReference;
        }

        public override void StateEnter()
        {
            animator.SetBool(isCalling, true);
            var playerPosition = GameObject.FindWithTag("Player").transform.position;
            var directionToPlayer = playerPosition - agent.transform.position;
            targetComponent.SetTarget(null);
            targetComponent.SetPoint(agent.transform.position + -directionToPlayer.normalized * 20f);
        }

        public override void StateExit()
        {
            animator.SetBool(isCalling, false);
            runawayTimerReference.StartTimer();
        }
    }
}