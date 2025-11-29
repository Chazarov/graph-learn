namespace Domain
{
    public interface GraphEdgeInterface<TNode>:GraphEdgeBaseInterface where TNode : GraphNodeInterface
    {
       
        public TNode GetSourceNode();
        public TNode GetTargetNode();

        public void SetSourseNode(TNode node);

        public void SetTargetNode(TNode node);

    }
}