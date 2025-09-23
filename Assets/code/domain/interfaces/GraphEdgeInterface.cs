namespace Domain
{
    public interface GraphEdgeInterface
    {
        public float GetWeight();
        public void SetWeight(float weight);
        public bool HasWeight();
        public GraphNodeInterface GetSourceNode();
        public GraphNodeInterface GetTargetNode();

        public void DeleteEdge();
        public bool IsParralel(GraphEdgeInterface other);
    }
}