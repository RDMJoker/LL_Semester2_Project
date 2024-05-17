using UnityEngine;

public class RainScriptTest : MonoBehaviour
{
    [SerializeField] float wetnessDensity;
    MeshRenderer meshRenderer;
    Material mainMaterial;
    Texture texture;
    
    void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        mainMaterial = new Material(meshRenderer.material);
        meshRenderer.material = mainMaterial;
        var bounds = meshRenderer.bounds;
        texture = new Texture2D((int)(bounds.extents.x * wetnessDensity),(int)(bounds.extents.z * wetnessDensity));
        mainMaterial.SetTexture("_WetnessMap",texture);
    }
}
