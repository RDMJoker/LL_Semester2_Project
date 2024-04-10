using System.Collections.Generic;
using KI;
using UnityEngine;

public abstract class State
{
    readonly List<Transition> Transitions;

    protected State()
    {
        Transitions = new List<Transition>();
    }

    public virtual void StateEnter()
    {
    }

    public virtual void StateExit()
    {
    }

    public virtual void Tick()
    {
    }

    public void AddTransition(Transition _transition)
    {
        Transitions.Add(_transition);
    }

    public bool CheckTransitions(out State _nextState)
    {
        foreach (var transition in Transitions)
        {
            if (transition.Condition())
            {
                _nextState = transition.NextState;
                return true;
            }
        }

        _nextState = null;
        return false;
    }
}