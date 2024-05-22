using UnityEngine;

namespace LL_Unity_Utils.Scriptables
{
    [CreateAssetMenu(menuName = "Scriptables/VFX/VFXSpawner", fileName = "NewVFXSpawner")]
    public class VFXSpawner : ScriptableObject
    {
        [SerializeField] GameObject vfxPrefab;

        public void Spawn(Vector3 _position)
        {
            Instantiate(vfxPrefab, _position + Vector3.up, vfxPrefab.transform.rotation);
        }
    }
}