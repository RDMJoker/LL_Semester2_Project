using LL_Unity_Utils.Misc;
using UnityEngine;

namespace KI.Non_Humanoid
{
    public class ProtectState : State
    {
        readonly TargetComponent protectionTarget;
        readonly Transform transform;
        readonly float rotateSpeed;
        
        public ProtectState(TargetComponent _protectionTarget, Transform _wispAgent, float _rotateSpeed)
        {
            protectionTarget = _protectionTarget;
            transform = _wispAgent;
            rotateSpeed = _rotateSpeed;
        }

        public override void Tick()
        {
            transform.RotateAround(protectionTarget.TargetPosition, Vector3.up, rotateSpeed * Time.fixedDeltaTime);
        }

        public override void StateExit()
        {
            protectionTarget.SetTarget(null);
        }
    }
}