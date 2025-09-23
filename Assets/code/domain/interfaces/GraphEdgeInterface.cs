namespace domain
{
    public interface GraphEdgeInterface
    {
        public float GetWeight();
        public void SetWeight(float weight);
        public bool HasWeight();
        public GraphNode GetSourceNode();
        public GraphNode GetTargetNode();

        public void DeleteEdge();
        public bool IsParralel(GraphEdgeInterface other);
    }
}