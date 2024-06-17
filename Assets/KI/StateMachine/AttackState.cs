using Cinemachine;
using UnityEngine;
using UnityEngine.AI;

namespace KI
{
    public class AttackState : AnimationState
    {
        static readonly int isWalking = Animator.StringToHash("isWalking");
        static readonly int isIdle = Animator.StringToHash("isIdle");
        static readonly int isAttacking = Animator.StringToHash("isAttacking");
        static readonly int attackSpeed = Animator.StringToHash("AttackSpeed");
        readonly Agent agent;

        public AttackState(Animator _animator, Agent _agent) : base(_animator)
        {
            agent = _agent;
        }

        public override void StateEnter()
        {
            animator.SetBool(isAttacking, true);
            animator.SetBool(isWalking, false);
            animator.SetBool(isIdle, false);
            
             
        }

        public override void StateExit()
        {
            animator.SetBool(isAttacking, false);
        }

        public override void Tick()
        {
            // Debug.Log(animator.GetCurrentAnimatorClipInfo(0)[0].clip.name);
            var clipLength = animator.GetCurrentAnimatorClipInfo(0)[0].clip.length;
            animator.SetFloat(attackSpeed, agent.AttackSpeed / clipLength);
        }
    }
}