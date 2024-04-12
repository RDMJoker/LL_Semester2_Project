using UnityEngine;
using UnityEngine.AI;

public class AnimationTests : MonoBehaviour
{
    const float AgentSpeedMultiplier = 6f;
    const float MaxAnimationPlaySpeed = 2f;
    public Animator Animator;
    public float Speed;
    NavMeshAgent agent;

    static readonly int speedFloat = Animator.StringToHash("SpeedFloat");

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void FixedUpdate()
    {
        Animator.SetFloat(speedFloat, Mathf.Clamp(Speed, 0f, MaxAnimationPlaySpeed));
        agent.speed = Speed * AgentSpeedMultiplier;
    }
}