using GraphMaster;

namespace Domain
{
    public interface GraphEdgeBaseInterface: GraphPartInterface
    {
        public float GetWeight();
        public void SetWeight(float weight);
        public bool HasWeight();

        public void SetName(string name);

    }
}


