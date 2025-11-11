using GraphMaster;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Domain
{
    public interface GraphInterface <TNode, TEdge> where TNode : GraphNodeInterface where TEdge : GraphEdgeInterface
    {
        public bool HasNodes();


        public List<TNode> GetNodes();
        public List<TEdge> GetEdges();

        public TNode GetNode(string name);

        public int GetNodeCount();



        public TNode AddNode(TNode node);
        
        public TEdge AddEdge(TEdge edge);

        public void DeleteNode(string name);

        
    }
}