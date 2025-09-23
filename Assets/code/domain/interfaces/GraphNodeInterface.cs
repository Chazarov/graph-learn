using System.Collections;
using System.Collections.Generic;

namespace Domain
{
    public interface GraphNodeInterface
    {
        public int GetNumber();
        public string GetName();
        public string GetDescription();

        public void DisconnectEdge(GraphEdgeInterface edge);
        public List<GraphEdgeInterface> GetEdges();
        public GraphInterface GetGraph();
        public void AddEdge(GraphEdgeInterface edge);
        public void DeleteNode();
        public void SetName(string name);
        public void SetDescription(string description);
    }
}