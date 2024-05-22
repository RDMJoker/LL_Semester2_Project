using LL_Unity_Utils.Misc;
using LL_Unity_Utils.Timers;
using Spawner;
using UnityEngine;
using UnityEngine.AI;

namespace KI
{
    public class CallReinforcementState : AnimationState
    {
        readonly TargetComponent targetComponent;
        readonly NavMeshAgent agent;
        readonly Timer runawayTimerReference;

        static readonly int isCalling = Animator.StringToHash("isCalling");
        
        public CallReinforcementState(Animator _animator, TargetComponent _targetComponent, NavMeshAgent _agent, Timer _runawayTimerReference) : base(_animator)
        {
            targetComponent = _targetComponent;
            agent = _agent;
            runawayTimerReference = _runawayTimerReference;
        }

        public override void StateEnter()
        {
            animator.SetBool(isCalling, true);
            var playerPosition = GameObject.FindWithTag("Player").transform.position;
            var directionToPlayer = playerPosition - agent.transform.position;
            targetComponent.SetPoint(agent.transform.position + -directionToPlayer.normalized * 20f);
        }

        public override void StateExit()
        {
            animator.SetBool(isCalling, false);
            runawayTimerReference.StartTimer();
        }
    }
}