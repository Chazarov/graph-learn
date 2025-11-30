using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Domain;
using System;
using System.Linq;
using Unity.VisualScripting;
using System.Xml.Linq;
using Unity.Collections;

namespace GraphMaster
{
    public class Graph<TNode, TEdge> : GraphInterface<TNode, TEdge> where TNode : Domain.GraphNodeInterface where TEdge : Domain.GraphEdgeInterface<TNode>
    {


        private List<TNode> nodes = new List<TNode>();
        private List<TEdge> edges = new List<TEdge>();
        private Dictionary<string, TNode> nodesMap = new Dictionary<string, TNode>();
        private Dictionary<string, TEdge> edgesMap = new Dictionary<string, TEdge>();

        //A separate edgesMap structure is needed to track parallel edges.
        private Dictionary<NodePair, List<TEdge>> nodesPairMap = new Dictionary<NodePair, List<TEdge>>();

        private bool parralelEdgesAreAllowed = false;
        private bool loopsAreAllowed = false;

        public void SetParralelEdgesAllowed(bool allowed)
        {
            parralelEdgesAreAllowed = allowed;
        }

        public bool GetParralelEdgesAllowed()
        {
            return parralelEdgesAreAllowed;
        }

        public TNode GetNode(string name)
        {
            if (!nodesMap.TryGetValue(name, out var node))
            {
                throw new NotFoundException("Node", name, "Graph");
            }
            return node;
        }

        public TEdge GetEdge(string name)
        {
            if (!edgesMap.TryGetValue(name, out var edge))
            {
                throw new NotFoundException("Edge", name, "Graph");
            }
            return edge;
        }

        public List<TNode> GetNodes()
        {
            return new List<TNode>(this.nodes);
        }

        public List<TEdge> GetEdges()
        {
            return new List<TEdge>(this.edges);
        }


        public TEdge AddEdge(TEdge edge)
        {
            if (edges.Contains(edge))
            {
                throw new DublicateException("It is not possible to add the same edge twice.");
            }

            string sourseName = edge.GetSourceNode().GetName();
            string targetName = edge.GetTargetNode().GetName();
            string edgeName = edge.GetName();


            CheckPossibilityOfAddingAnEdge(sourseName, targetName, edgeName);
            

            edges.Add(edge);

            NodePair pair = new NodePair(sourseName, targetName);
            if (!nodesPairMap.ContainsKey(pair))
            {
                nodesPairMap[pair] = new List<TEdge>();
            }
            nodesPairMap[pair].Add(edge);
            edgesMap[edge.GetName()] = edge;

            return edge;
        }

        public void CheckPossibilityOfAddingAnEdge(string sourseName, string targetName, string edgeName)
        {

            if (this.edgesMap.ContainsKey(edgeName))
            {
                throw new DublicateException("It is not possible to add the edge with same name twice.");
            }

            if (sourseName == targetName)
            {
                if(!this.loopsAreAllowed)
                {
                    throw new LoopsNotAllowed($"it is impossible to create an edge that starts and ends at the same vertex. Loops are not  allowed");
                }
            }

            NodePair pair = new NodePair(sourseName, targetName);
            if (!this.parralelEdgesAreAllowed)
            {
                if (nodesPairMap.ContainsKey(pair))
                {
                    throw new ParralelEdgesNotAllowed($" The graph already has an edge connecting nodes {sourseName} and {targetName}. Currently, parallel edges are prohibited in the graph.");
                }
                
            }  
        }

        public TNode AddNode(TNode node)
        {
            if (nodes.Contains(node))
            {
                throw new DublicateException("It is not possible to add the same node twice.");
            }
            if (nodesMap.ContainsKey(node.GetName()))
            {
                throw new DublicateException("It is not possible to add the node with same name twice.");
            }
            nodesMap.Add(node.GetName(), node);
            nodes.Add(node);
            return node;
        }


        public void DeleteNode(string name)
        {
            if (!nodesMap.TryGetValue(name, out var node))
            {
                throw new NotFoundException("Node", name, "Graph");
            }
            this.nodes.Remove(node);
            this.nodesMap.Remove(name);
        }

        public bool HasNodes()
        {
            return nodes.Count > 0;
        }

        public int GetNodeCount()
        {
            return nodes.Count;
        }

        public void DeleteEdge(TEdge edge)
        {
            throw new NotImplementedException();
        }

        public struct NodePair : IEquatable<NodePair>
        {
            public string First { get; }
            public string Second { get; }

            public NodePair(string a, string b)
            {
                First = string.CompareOrdinal(a, b) < 0 ? a : b;
                Second = string.CompareOrdinal(a, b) < 0 ? b : a;
            }

            public override int GetHashCode() => HashCode.Combine(First, Second);

            public override bool Equals(object obj) => obj is NodePair other && Equals(other);

            public bool Equals(NodePair other) => First == other.First && Second == other.Second;
        }

    }
}

