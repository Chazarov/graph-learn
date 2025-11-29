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
    public class Graph<TNode, TEdge> : GraphInterface<TNode, TEdge> where TNode : Domain.GraphNodeInterface where TEdge : Domain.GraphEdgeInterface
    {


        private List<TNode> nodes = new List<TNode>();
        private List<TEdge> edges = new List<TEdge>();
        private Dictionary<string, TNode> nodesMap = new Dictionary<string, TNode>();

        //A separate edgesMap structure is needed to track parallel edges.
        private Dictionary<NodePair, List<TEdge>> edgesMap = new Dictionary<NodePair, List<TEdge>>();

        private bool parralelEdgesAreAllowed = false;

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
                throw new NodeNotFoundException(name);
            }
            return node;
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
            
            if (!parralelEdgesAreAllowed)
            {
                if (CheckParrallel(edge))
                {
                    throw new ParralelEdgesNotAllowed($" The graph already has an edge connecting nodes {edge.GetSourceNode().GetName()} and {edge.GetTargetNode().GetName()}");
                }
            }

            edges.Add(edge);

            NodePair pair = new NodePair(edge.GetSourceNode().GetName(), edge.GetTargetNode().GetName());
            if (!edgesMap.ContainsKey(pair))
            {
                edgesMap[pair] = new List<TEdge>();
            }
            edgesMap[pair].Add(edge);

            return edge;
        }

        public bool CheckParrallel(TEdge edge)
        {
            string sourseName = edge.GetSourceNode().GetName();
            string targetName = edge.GetTargetNode().GetName();

            NodePair pair = new NodePair(sourseName, targetName);

            return this.edgesMap.ContainsKey(pair);
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
                throw new NodeNotFoundException(name);
            }
            List<GraphEdgeInterface> nodeEdges = node.GetEdges();
            foreach (var edge in nodeEdges)
            {
                edge.GetSourceNode().DisconnectEdge(edge);
                edge.GetTargetNode().DisconnectEdge(edge);
                this.edges.Remove((TEdge)edge);

                NodePair pair = new NodePair(edge.GetSourceNode().GetName(), edge.GetTargetNode().GetName());
                if (edgesMap.ContainsKey(pair))
                {
                    edgesMap[pair].Remove((TEdge)edge);
                    if (edgesMap[pair].Count == 0)
                    {
                        edgesMap.Remove(pair);
                    }
                }
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

