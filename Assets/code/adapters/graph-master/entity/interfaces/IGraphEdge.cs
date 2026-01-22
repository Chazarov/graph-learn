namespace GraphMaster.Entity
{
    public interface IGraphEdge<TNode>: IGraphPart where TNode : IGraphNode
    {
       
        public TNode GetSourseNode();
        public TNode GetTargetNode();

        public string GetSourseName();

        public string GetTargetName();

        public void SetSourseNode(TNode node);

        public void SetTargetNode(TNode node);

    }
}