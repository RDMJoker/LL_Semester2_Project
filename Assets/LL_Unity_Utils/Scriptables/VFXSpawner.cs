using UnityEngine;

namespace LL_Unity_Utils.Scriptables
{
    [CreateAssetMenu(menuName = "Scriptables/VFX/VFXSpawner", fileName = "NewVFXSpawner")]
    public class VFXSpawner : ScriptableObject
    {
        [SerializeField] GameObject vfxPrefab;
        [SerializeField] Vector3 positionOffset = Vector3.up;

        public void Spawn(Vector3 _position, out GameObject _spawnedObject)
        {
           _spawnedObject = Instantiate(vfxPrefab, _position + positionOffset, vfxPrefab.transform.rotation);
        }

        public void Spawn(Vector3 _position)
        {
            Instantiate(vfxPrefab, _position + positionOffset, vfxPrefab.transform.rotation);
        }

    }
}