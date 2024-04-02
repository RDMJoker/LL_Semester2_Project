using System;

namespace KI
{
    public class Transition
    {
        Func<bool> condition;
        State nextState;

        public Transition(State _nextState, Func<bool> _condition)
        {
            condition = _condition;
            nextState = _nextState;
        }
    }
}