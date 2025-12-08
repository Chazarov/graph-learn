using GraphMaster;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Domain
{
    public interface GraphInterface <TNode, TEdge> where TNode : GraphNodeInterface where TEdge : GraphEdgeBaseInterface
    {
        public bool HasNodes();


        public List<TNode> GetNodes();
        public List<TEdge> GetEdges();

        public TNode GetNode(string name);

        public TEdge GetEdge(string name);

        public int GetNodeCount();

        public void CheckPossibilityOfAddingAnEdge(string sourseName, string targetName, string edgeName);



        public TNode AddNode(TNode node);
        
        public TEdge AddEdge(TEdge edge);

        public void DeleteNode(string name);

        public void DeleteEdge(TEdge edge);



        public void SetParralelEdgesAllowed(bool value);

        public void SetDirected(bool value);


    }
}