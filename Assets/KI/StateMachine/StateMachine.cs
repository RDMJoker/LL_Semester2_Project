﻿using UnityEngine;

namespace KI
{
    public class StateMachine
    {
        State currentState;
        GameObject agent;

        public StateMachine(State _startState, GameObject _agent)
        {
            currentState = _startState;
            currentState.StateEnter();
            agent = _agent;
        }

        public void CheckSwapState()
        {
            if (currentState.CheckTransitions(out var nextState))
            {
                currentState.StateExit();
                currentState = nextState;
                currentState.StateEnter();
            }
            else
            {
                currentState.Tick();
            }
        }
    }
}