using UnityEngine;
using UnityEngine.AI;

public class Testscript : MonoBehaviour
{
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Camera mainCamera;

    void Update()
    {
        //agent.velocity = Vector3.zero;
        if (Input.GetMouseButton(0))
        {
            var cameraRay = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit raycastHit;
            if (Physics.Raycast(cameraRay, out raycastHit))
            {
                
                agent.SetDestination(raycastHit.point);
            }
        }
    }
}