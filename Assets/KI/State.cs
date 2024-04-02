using UnityEngine;

public abstract class State
{
    protected State currentState;


    protected State( )
    {
        currentState = this;
    }

    protected virtual void StateEnter()
    {
        
    }

    protected virtual void StateExit()
    {
        
    }

    protected virtual void Tick()
    {
        
    }
    
}