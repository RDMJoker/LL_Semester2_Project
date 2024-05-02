using UnityEngine;

namespace KI
{
    public class StateMachine
    {
        State currentState;
        GameObject agent;
        bool debug;

        public StateMachine(State _startState, GameObject _agent, bool _debug)
        {
            currentState = _startState;
            currentState.StateEnter();
            agent = _agent;
            debug = _debug;
        }

        public void CheckSwapState()
        {
            if (currentState.CheckTransitions(out var nextState))
            {
                currentState.StateExit();
                if (debug) Debug.Log(agent.name + "Leaving State: " + currentState);
                currentState = nextState;
                if (debug) Debug.Log(agent.name + "Enter State: " + currentState);
                currentState.StateEnter();
            }
            else
            {
                currentState.Tick();
            }
        }
    }
}