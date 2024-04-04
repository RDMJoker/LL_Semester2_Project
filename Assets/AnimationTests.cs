using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AnimationTests : MonoBehaviour
{
    [SerializeField] float agentSpeedMultiplier;
    public Animator Animator;
    public float Speed;
    public float AttackSpeed;

    NavMeshAgent agent;
    public bool IsWalking;

    static readonly int NewTrigger = Animator.StringToHash("NewTrigger");
    static readonly int SpeedFloat = Animator.StringToHash("SpeedFloat");
    static readonly int CurrentVelocityX = Animator.StringToHash("CurrentVelocityX");
    static readonly int CurrentVelocityY = Animator.StringToHash("CurrentVelocityY");
    static readonly int isWalking = UnityEngine.Animator.StringToHash("isWalking");

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Animator.SetTrigger(NewTrigger);
        }
    }

    void FixedUpdate()
    {
        Animator.SetFloat(SpeedFloat, Mathf.Clamp(Speed, 0f, 2f));
        //Animator.SetFloat(SpeedFloat, Speed);
        agent.speed = Speed * agentSpeedMultiplier;
        SetWalking(agent.hasPath);
        Animator.SetFloat(CurrentVelocityX, agent.velocity.x);
        Animator.SetFloat(CurrentVelocityY, agent.velocity.z);
        Animator.SetBool(isWalking, IsWalking);
    }

    void SetWalking(bool _value)
    {
        IsWalking = _value;
    }
}