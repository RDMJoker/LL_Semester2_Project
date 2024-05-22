using UnityEngine;
using UnityEngine.Rendering;

namespace LL_Unity_Utils.Misc
{
    public class RotateToCamera : MonoBehaviour
    {
        [SerializeField] Vector3 rotationOffset;

        void OnEnable()
        {
            RenderPipelineManager.beginCameraRendering += RotateObject;
        }

        void OnDisable()
        {
            RenderPipelineManager.beginCameraRendering -= RotateObject;
        }

        void RotateObject(ScriptableRenderContext _context, Camera _camera)
        {
            transform.LookAt(transform.position + _camera.transform.rotation * Vector3.forward, _camera.transform.rotation * Vector3.up);
            transform.Rotate(rotationOffset, Space.Self);
        }
    }
}