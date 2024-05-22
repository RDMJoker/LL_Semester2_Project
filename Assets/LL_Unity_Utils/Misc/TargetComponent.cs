using UnityEngine;

namespace LL_Unity_Utils.Misc
{
    public class TargetComponent
    {
        Vector3 position;
        Transform targetTransform;
        public Vector3 TargetPosition => targetTransform == null ? position : targetTransform.position;
        
        public void SetTarget(Transform _target)
        {
            targetTransform = _target;
        }

        public void SetPoint(Vector3 _point)
        {
            targetTransform = null;
            position = _point;
        }
    }
}