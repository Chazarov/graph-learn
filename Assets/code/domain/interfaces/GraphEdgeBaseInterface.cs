namespace Domain
{
    public interface GraphEdgeBaseInterface
    {
        public float GetWeight();
        public void SetWeight(float weight);
        public bool HasWeight();

        public string GetName();
        public void SetName(string name);

    }
}


