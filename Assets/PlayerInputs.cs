using KI;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputs : MonoBehaviour
{
    [SerializeField] Camera mainCamera;
    [SerializeField] LayerMask hitboxLayer;
    PlayerAgent playerAgent;
    bool hasInput;
    Vector2 mousePosition;
    Collider test;

    void Awake()
    {
        playerAgent = GetComponent<PlayerAgent>();
    }

    void Update()
    {
        if (hasInput)
        {
            var cameraRay = mainCamera.ScreenPointToRay(mousePosition);
            RaycastHit raycastHit;
            if (Physics.Raycast(cameraRay, out raycastHit))
            {
                playerAgent.SetTargetComponentPosition(raycastHit.point);
                playerAgent.IsWalking = true;
            }
        }
    }

    public void ReadMousePosition(InputAction.CallbackContext _callbackContext)
    {
        mousePosition = _callbackContext.ReadValue<Vector2>();
    }

    public void MoveTo(InputAction.CallbackContext _callbackContext)
    {
        hasInput = _callbackContext.phase != InputActionPhase.Canceled;
    }

    public void HitEnemy(InputAction.CallbackContext _callbackContext)
    {
        if (_callbackContext.phase != InputActionPhase.Started) return;
        var cameraRay = mainCamera.ScreenPointToRay(mousePosition);
        RaycastHit raycastHit;
        Physics.Raycast(cameraRay, out raycastHit, hitboxLayer);
        if (raycastHit.collider.gameObject.TryGetComponent(out EnemyAgent target) && Vector3.Distance(playerAgent.transform.position, target.transform.position) <= playerAgent.AttackRange)
        {
            target.OnHit(playerAgent);
        }
    }
}