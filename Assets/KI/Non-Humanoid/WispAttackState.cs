using LL_Unity_Utils.Misc;
using UnityEngine;

namespace KI.Non_Humanoid
{
    public class WispAttackState : State
    {
        TargetComponent target;
        readonly Animator animator;
        readonly float agentAttackSpeed;
        static readonly int isAttacking = Animator.StringToHash("IsAttacking");
        static readonly int attackSpeed = Animator.StringToHash("AttackSpeed");

        public WispAttackState(TargetComponent _target, Animator _animator, float _agentAttackSpeed)
        {
            target = _target;
            animator = _animator;
            agentAttackSpeed = _agentAttackSpeed;
        }

        public override void StateEnter()
        {
              animator.SetBool(isAttacking,true);
        }

        public override void StateExit()
        {
             animator.SetBool(isAttacking,false);
        }

        public override void Tick()
        {
            var clipLength = animator.GetCurrentAnimatorClipInfo(0)[0].clip.length;
            animator.SetFloat(attackSpeed, agentAttackSpeed / clipLength);
        }
    }
}