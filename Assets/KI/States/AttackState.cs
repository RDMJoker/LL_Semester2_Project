using UnityEngine;

namespace KI
{
    public class AttackState : AnimationState
    {
        public AttackState(Animator _animator) : base(_animator)
        {
        }

        public override void StateEnter()
        {
            animator.SetBool(IsAttacking,true);
            animator.SetBool(IsWalking, false);
        }

        public override void StateExit()
        {
            animator.SetBool(IsAttacking,false);
        }

        public override void Tick()
        {
            base.Tick();
        }
    }
}