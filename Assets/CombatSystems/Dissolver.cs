using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace CombatSystems
{
    
    public class Dissolver : MonoBehaviour
    {
        [SerializeField] float dissolveTime = 5f;
        [SerializeField] float despawnDissolveThreshhold = 0.9f;
        const float DissolveValue = 1;
        public void StartDissolve()
        {
            StartCoroutine(Dissolve());
        }

        IEnumerator Dissolve()
        {
            var meshRenderer = GetComponent<MeshRenderer>();
            var material = meshRenderer.material;
            material.DOFloat(DissolveValue, "_DissolveProgress", dissolveTime);
            yield return new WaitUntil(() => material.GetFloat("_DissolveProgress") > despawnDissolveThreshhold);
            Destroy(gameObject);
        }
    }
}