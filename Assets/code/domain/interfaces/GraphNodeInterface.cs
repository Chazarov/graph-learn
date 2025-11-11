using System.Collections;
using System.Collections.Generic;

namespace Domain
{
    public interface GraphNodeInterface
    {
        public string GetName();
        public string GetDescription();

        public void DisconnectEdge(GraphEdgeInterface edge);
        public List<GraphEdgeInterface> GetEdges();
        public void AddEdge(GraphEdgeInterface edge);

        public void SetName(string name);
        public void SetDescription(string description);

    }
}