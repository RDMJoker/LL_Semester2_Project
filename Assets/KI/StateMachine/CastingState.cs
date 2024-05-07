using Unity.VisualScripting;
using UnityEngine;

namespace KI
{
    public class CastingState : AnimationState
    {
        static readonly int isCasting = Animator.StringToHash("isCasting");

        Agent castingAgent;
        TargetComponent targetComponent;

        public CastingState(Animator _animator, Agent _castingAgent, TargetComponent _targetComponent) : base(_animator)
        {
            castingAgent = _castingAgent;
            targetComponent = _targetComponent;
        }

        public override void StateEnter()
        {
            animator.SetBool(isCasting, true);
            castingAgent.transform.LookAt(targetComponent.TargetPosition);
        }
    }
}