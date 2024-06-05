using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace KI.Non_Humanoid
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class WispController : MonoBehaviour
    {
        [SerializeField] float flyRange;
        [SerializeField] float flySpeed;
        [SerializeField] float flyPointDistanceThreshhold;
        Vector3 spawnPosition;
        NavMeshAgent agent;

        void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            agent.speed = flySpeed;
            spawnPosition = transform.position;
            GetRandomPoint();
        }

        void FixedUpdate()
        {
            if (agent.remainingDistance <= agent.stoppingDistance || agent.hasPath == false) GetRandomPoint();
        }

        void GetRandomPoint()
        {
            Vector3 randomPoint;
            do
            {
                var unitSphere = Random.insideUnitSphere * flyRange;
                randomPoint = new Vector3(unitSphere.x, 0, unitSphere.z);
                randomPoint += spawnPosition;
            } while (!NavMesh.SamplePosition(randomPoint, out _, agent.radius * 2, agent.areaMask) || Vector3.Distance(transform.position, randomPoint) < flyPointDistanceThreshhold);

            agent.SetDestination(randomPoint);
        }
        void OnValidate()
        {
            flyPointDistanceThreshhold = Mathf.Clamp(flyPointDistanceThreshhold, 1, flyRange - 1);
        }
        
        void OnDrawGizmos()
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(transform.position, flyRange);
            // Gizmos.color = Color.yellow;
            // Gizmos.DrawSphere(transform.position + Vector3.up, AttackRange);
        }
        
    }
}