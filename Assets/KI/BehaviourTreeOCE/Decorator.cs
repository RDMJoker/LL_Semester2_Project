using Unity.VisualScripting;

namespace KI.BehaviourTree
{
    public abstract class Decorator : Node
    {
        Node childNode;

        protected Decorator(Node _child)
        {
            childNode = _child;
        }

        public override NodeState Evaluate()
        {
            return childNode.Evaluate();
        }
    }
}