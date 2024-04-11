using System;
using System.Linq;
using KI;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class PlayerInputs : MonoBehaviour
{
    [SerializeField] NavMeshAgent navMeshAgent;
    [SerializeField] Camera mainCamera;
    PlayerAgent playerAgent;
    bool hasInput;
    Vector2 mousePosition;
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
                navMeshAgent.SetDestination(raycastHit.point);
            }
        }
    }

    public void MoveTo(InputAction.CallbackContext _callbackContext)
    {
        hasInput = _callbackContext.phase != InputActionPhase.Canceled;
        if (hasInput) mousePosition = _callbackContext.ReadValue<Vector2>();
    }

    public void HitEnemy(InputAction.CallbackContext _callbackContext)
    {
        if (_callbackContext.phase != InputActionPhase.Started) return;
        var overlap = Physics.OverlapSphere(this.transform.position, playerAgent.AttackRange, playerAgent.enemyMask);
        if (overlap.Length > 0)
        {
            Debug.Log("Ich schlage einen Gegner!");
            overlap.First().gameObject.TryGetComponent(out Agent enemyAgent);
            enemyAgent.OnHit(playerAgent);
        }
        else
        {
            Debug.Log("Ich finde keinen Gegner :( ");
        }
    }
}