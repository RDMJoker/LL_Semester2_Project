using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.PlayerLoop;

namespace KI
{
    public class PatrolState : WalkToPointState
    {
        readonly Action calculatePathAction;

        public PatrolState(NavMeshAgent _agent, TargetComponent _target, Animator _animator, Action _calculatePathAction) : base(_agent, _target, _animator)
        {
            calculatePathAction = _calculatePathAction;
        }

        public override void StateEnter()
        {
            calculatePathAction();
            base.StateEnter();
        }
    }
}