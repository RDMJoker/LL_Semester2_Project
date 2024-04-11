using UnityEngine;

namespace KI
{
    public class AnimationState : State
    {
        protected readonly Animator animator;

        protected AnimationState(Animator _animator)
        {
            animator = _animator;
        }
    }
}