using UnityEngine;

namespace KI
{
    public class StunnedState : AnimationState
    {
        readonly Timer stunTimer;
        readonly Agent agent;

        static readonly int isStunned = Animator.StringToHash("isStunned");
        static readonly int stunAnimSpeed = Animator.StringToHash("StunAnimSpeed");

        public StunnedState(Animator _animator, Timer _stunTimer,Agent _agent) : base(_animator)
        {
            stunTimer = _stunTimer;
            agent = _agent;
        }

        public override void StateEnter()
        {
            animator.SetBool(isStunned, true);
            stunTimer.StartTimer();
        }

        public override void StateExit()
        {
            animator.SetBool(isStunned, false);
        }

        public override void Tick()
        {
            var clipLength = animator.GetCurrentAnimatorClipInfo(0)[0].clip.length;
            animator.SetFloat(stunAnimSpeed, agent.StunDuration / clipLength);
        }
    }
}