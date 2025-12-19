namespace Domain
{
    public interface GraphEdgeInterface<TNode>:GraphEdgeBaseInterface where TNode : GraphNodeInterface
    {
       
        public TNode GetSourseNode();
        public TNode GetTargetNode();

        public string GetSourseName();

        public string GetTargetName();

        public void SetSourseNode(TNode node);

        public void SetTargetNode(TNode node);

    }
}