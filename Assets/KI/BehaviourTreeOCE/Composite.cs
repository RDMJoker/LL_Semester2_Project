using System.Collections.Generic;

namespace KI.BehaviourTree
{
    public abstract class Composite : Node
    {
        protected List<Node> childNodes;

        protected Composite(List<Node> _childNodes)
        {
            childNodes = _childNodes;
        }
    }
}