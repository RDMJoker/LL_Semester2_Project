using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTests : MonoBehaviour
{
    public Animator Animator;
    public float Speed;
    public float AttackSpeed;
    
    static readonly int NewTrigger = Animator.StringToHash("NewTrigger");
    static readonly int SpeedFloat = Animator.StringToHash("SpeedMultiplier");
    static readonly int CurrentMoveSpeed = Animator.StringToHash("CurrentMoveSpeed");


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Animator.SetTrigger(NewTrigger);
        }

        Animator.SetFloat(SpeedFloat, Speed);
    }
}