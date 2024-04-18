using UnityEngine;

namespace KI
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
            position = _point;
        }
    }
}