using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace KI
{
    public class AnimationState : State
    {
        protected readonly Animator animator;
        protected static readonly int CurrentVelocityX = Animator.StringToHash("CurrentVelocityX");
        protected static readonly int CurrentVelocityZ = Animator.StringToHash("CurrentVelocityZ");
        protected static readonly int IsWalking = Animator.StringToHash("isWalking");
        protected static readonly int IsIdle = Animator.StringToHash("isIdle");
        protected static readonly int IsAttacking = Animator.StringToHash("isAttacking");

        protected AnimationState(Animator _animator)
        {
            animator = _animator;
        }
    }
}