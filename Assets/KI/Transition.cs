using System;

namespace KI
{
    public class Transition
    {
        public readonly Func<bool> Condition;
        public readonly State NextState;

        public Transition(State _nextState, Func<bool> _condition)
        {
            Condition = _condition;
            NextState = _nextState;
        }
    }
}