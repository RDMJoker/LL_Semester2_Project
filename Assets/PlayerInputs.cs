using System;
using CombatSystems.Skills;
using DefaultNamespace.Enums;
using KI;
using Scriptables.SceneLoader;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerInputs : MonoBehaviour
{
    [SerializeField] Camera mainCamera;
    [SerializeField] LayerMask mouseRayLayer;
    PlayerAgent playerAgent;
    bool hasInput;
    bool hasCastInput;
    Vector2 mousePosition;
    Collider test;
    Ray cameraRay;
    RaycastHit raycastHit;
    SkillCaster skillCaster;

    public static Action<Vector3> OnSkillButtonPressed;

    void Awake()
    {
        playerAgent = GetComponent<PlayerAgent>();
        skillCaster = GetComponent<SkillCaster>();
    }

    void Update()
    {
        if (hasInput)
        {
            cameraRay = mainCamera.ScreenPointToRay(mousePosition);
            if (Physics.Raycast(cameraRay, out raycastHit, Mathf.Infinity ,mouseRayLayer))
            {
                playerAgent.SetTargetComponentPosition(raycastHit.point);
                playerAgent.IsWalking = true;
            }
        }
        if (hasCastInput)
        {
            Casting();
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
        hasCastInput = _callbackContext.phase != InputActionPhase.Canceled;


        //transform.LookAt(raycastHit.point);
        
    }

    void Casting()
    {
        if (!skillCaster.GetCurrentTimerStatus())
        {
            Debug.Log("Still on cooldown!");
            return;
        }

        playerAgent.IsWalking = false;
        playerAgent.IsCasting = true;
        Physics.Raycast(mainCamera.ScreenPointToRay(mousePosition), out raycastHit);
        playerAgent.SetTargetComponentPosition(raycastHit.point);
        OnSkillButtonPressed.Invoke(raycastHit.point);
    }

    public void Interact(InputAction.CallbackContext _callbackContext)
    {
        if (_callbackContext.phase != InputActionPhase.Started) return;
        playerAgent.Interact();
    }

    public void ReturnToMenu(InputAction.CallbackContext _callbackContext)
    {
        if (_callbackContext.phase != InputActionPhase.Started) return;
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
        SceneManager.LoadScene((int)EScenes.MainMenu);
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