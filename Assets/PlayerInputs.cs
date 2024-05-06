using System;
using KI;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class PlayerInputs : MonoBehaviour
{
    [SerializeField] Camera mainCamera;
    [SerializeField] LayerMask hitboxLayer;
    PlayerAgent playerAgent;
    bool hasInput;
    Vector2 mousePosition;
    Collider test;
    Ray cameraRay;
    RaycastHit raycastHit;

    public static Action OnSkillButtonPressed;

    void Awake()
    {
        playerAgent = GetComponent<PlayerAgent>();
        
    }

    void Update()
    {
        if (hasInput)
        {
            cameraRay = mainCamera.ScreenPointToRay(mousePosition);
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

    public void CastSkill(InputAction.CallbackContext _callbackContext)
    {
        if (_callbackContext.phase != InputActionPhase.Started) return;
        Physics.Raycast(mainCamera.ScreenPointToRay(mousePosition), out raycastHit);
        transform.LookAt(raycastHit.point);
        OnSkillButtonPressed.Invoke();
    }
    public void PointAndClick(InputAction.CallbackContext _callbackContext)
    {
        // var cameraRay = mainCamera.ScreenPointToRay(mousePosition);
        // RaycastHit raycastHit;
        // Physics.Raycast(cameraRay, out raycastHit, hitboxLayer);
        // if (raycastHit.collider.gameObject.TryGetComponent(out EnemyAgent target) && Vector3.Distance(playerAgent.transform.position, target.transform.position) <= playerAgent.AttackRange)
        // {
        //     target.OnHit(playerAgent);
        // }
    }
}