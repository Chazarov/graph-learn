using GraphMaster;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Domain
{
    public interface GraphInterface< TNode> where TNode : GraphNodeInterface
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

        public List<TNode> GetNodes();

        public int GetNodeCount();


        public TNode AddNode(string name);

        public TNode AddNode(string name, string description);

        internal TNode addNode(GraphNodeInterface node);


        public void DeleteNode(int nodeNumber);

        
    }
}