using System;
using UnityEngine;

namespace CombatSystems.Skills
{
    public class EffectCollider : MonoBehaviour
    {
        public Action<GameObject> OnCollision;

        void OnParticleCollision(GameObject _collisionGameObject)
        {
            OnCollision.Invoke(_collisionGameObject);
        }
    }
}