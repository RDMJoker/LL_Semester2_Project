using System.Collections.Generic;

namespace KI.BehaviourTree
{
    public class Sequence : Composite
    {
        public Sequence(List<Node> _childNodes) : base(_childNodes)
        {
        }

        public override NodeState Evaluate()
        {
            foreach (var child in childNodes)
            {
                var childState = child.Evaluate();
                if (childState != NodeState.Success)
                {
                    return childState;
                }
            }

            return NodeState.Success;
        }
    }
}