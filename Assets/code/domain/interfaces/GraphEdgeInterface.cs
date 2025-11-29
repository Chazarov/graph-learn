namespace Domain
{
    public interface GraphEdgeInterface
    {
        public float GetWeight();
        public void SetWeight(float weight);
        public bool HasWeight();

        public string GetName();
        public void SetName(string name);
        public GraphNodeInterface GetSourceNode();
        public GraphNodeInterface GetTargetNode();

        public void SetSourseNode(GraphNodeInterface node);

        public void SetTargetNode(GraphNodeInterface node);

        public bool IsParralel(GraphEdgeInterface other);
    }
}