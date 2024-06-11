using Interface;
using UnityEngine;
using UnityEngine.Events;

namespace DefaultNamespace
{
    public class Interactable : MonoBehaviour, IInteractable
    {
        public UnityEvent OnInteraction;
        public void Interaction()
        {
            OnInteraction.Invoke();
        }

        public void DestroySelf()
        {
            Destroy(gameObject);
        }
    }
}