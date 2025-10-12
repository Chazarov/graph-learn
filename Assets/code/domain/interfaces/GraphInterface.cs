using GraphMaster;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Domain
{
    public interface GraphInterface
    {
        public bool AllowedParrallelEdges();
        public bool AllowedNegativeEdges();
        public bool AllowedLoops();
        public bool IsOriented();
        public bool IsWeighed();
        public bool HasNodes();

        public void MakeOriented();
        public void MakeParralel();
        public void MakeWeighed();
        public void AllowNegative();
        public void AllowLoops();

        public List<GraphNodeInterface> GetNodes();

        public int GetNodeCount();

        public GraphNodeInterface AddNode(GraphNodeInterface node);

        public GraphNodeInterface AddNode(string name);

        public GraphNodeInterface AddNode(string name, string description);


        public void DeleteNode(int nodeNumber);

        
    }
}