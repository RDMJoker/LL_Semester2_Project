namespace KI
{
    public class StateMachine
    {
        State currentState;

        public StateMachine(State _startState)
        {
            currentState = _startState;
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