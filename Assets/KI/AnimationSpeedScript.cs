using UnityEngine;
using UnityEngine.AI;

public class AnimationSpeedScript : MonoBehaviour
{
    const float AgentSpeedMultiplier = 6f;
    const float MaxAnimationPlaySpeed = 2f;
    Animator animator;
    public float Speed;
    NavMeshAgent agent;

    static readonly int moveSpeed = Animator.StringToHash("MoveSpeed");

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        animator.SetFloat(moveSpeed, Mathf.Clamp(Speed, 0f, MaxAnimationPlaySpeed));
        agent.speed = Speed * AgentSpeedMultiplier;
    }
}