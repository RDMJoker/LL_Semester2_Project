using UnityEngine;

namespace KI
{
    public class DeathState : AnimationState
    {
        static readonly int isDead = Animator.StringToHash("isDead");
        
        public DeathState(Animator _animator) : base(_animator)
        {
        }

        public override void StateEnter()
        {
            animator.SetBool(isDead, true);
        }
    }
}