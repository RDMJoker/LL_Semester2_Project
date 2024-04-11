using System.Collections.Generic;

namespace KI.BehaviourTree
{
    public class Selector : Composite
    {
        public Selector(List<Node> _childNodes) : base(_childNodes)
        {
        }

        public override NodeState Evaluate()
        {
            foreach (var child in childNodes)
            {
                var childState = child.Evaluate();
                if (childState != NodeState.Failure)
                {
                    return childState;
                }
            }
            
            return NodeState.Failure;
        }
    }
}