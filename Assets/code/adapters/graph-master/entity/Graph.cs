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


        private Dictionary<string, TNode> nodesMap = new Dictionary<string, TNode>();
        private Dictionary<string, TEdge> edgesMap = new Dictionary<string, TEdge>();

        // Algorithms with a directed weighed graph
        private Dictionary<string, Dictionary<string, List<TEdge>>> AdjacencyMap = new();

        private Dictionary<string, Dictionary<string, List<TEdge>>> ReversedAdjacencyMap = new();

        private bool parralelEdgesAreAllowed = false;
        private bool loopsAreAllowed = false;
        private bool isDirected = false;

        public void SetParralelEdgesAllowed(bool value)
        {
            parralelEdgesAreAllowed = value;
        }

        public void SetDirected(bool value)
        {
            isDirected = value;
        }

        public void SetLoopsAllowed(bool value)
        {
            loopsAreAllowed = value;
        }

        public bool GetParralelEdgesAllowed()
        {
            return parralelEdgesAreAllowed;
        }

        public bool GetIsDirected()
        {
            return isDirected;
        }

        public bool GetIsParralel()
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
            return new List<TNode>(this.nodesMap.Values);
        }

        public List<TEdge> GetEdges()
        {
            return new List<TEdge>(this.edgesMap.Values);
        }


        public TEdge AddEdge(TEdge edge)
        {
            string sourseName = edge.GetSourceNode().GetName();
            string targetName = edge.GetTargetNode().GetName();
            string edgeName = edge.GetName();
            float edgeWeight = edge.GetWeight();

            CheckPossibilityOfAddingAnEdge(sourseName, targetName, edgeName);
            
            edgesMap[edgeName] = edge;

            if (!AdjacencyMap[sourseName].ContainsKey(targetName)) AdjacencyMap[sourseName][targetName] = new();
            AdjacencyMap[sourseName][targetName].Add(edge);

            if (!ReversedAdjacencyMap[targetName].ContainsKey(sourseName)) ReversedAdjacencyMap[targetName][sourseName] = new();
            ReversedAdjacencyMap[targetName][sourseName].Add(edge);

            return edge;
        }
        public void DeleteEdge(string name)
        {
            TEdge edge = this.GetEdge(name);
            DeleteEdge(edge);
        }
        public void DeleteEdge(TEdge edge)
        {
            if (!edgesMap.ContainsKey(edge.GetName()))
            {
                throw new NotFoundException("Edge", edge.GetName(), "Graph");
            }

            string edgeName = edge.GetName();
            float edgeWeight = edge.GetWeight();
            string sourceName = edge.GetSourceNode().GetName();
            string targetName = edge.GetTargetNode().GetName();

            edgesMap.Remove(edgeName);

            if (AdjacencyMap.ContainsKey(sourceName) && AdjacencyMap[sourceName].ContainsKey(targetName))
            {
                AdjacencyMap[sourceName][targetName].Remove(edge);
                if (AdjacencyMap[sourceName][targetName].Count == 0)
                {
                    AdjacencyMap[sourceName].Remove(targetName);
                }
            }

            if (ReversedAdjacencyMap.ContainsKey(targetName) && ReversedAdjacencyMap[targetName].ContainsKey(sourceName))
            {
                ReversedAdjacencyMap[targetName][sourceName].Remove(edge);
                if (ReversedAdjacencyMap[targetName][sourceName].Count == 0)
                {
                    ReversedAdjacencyMap[targetName].Remove(sourceName);
                }
            }
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

            this.GetNode(sourseName);
            this.GetNode(targetName);

            if (!this.parralelEdgesAreAllowed)
            {
                
                if (AdjacencyMap[sourseName].ContainsKey(targetName) || ReversedAdjacencyMap[targetName].ContainsKey(sourseName))
                {
                    throw new ParralelEdgesNotAllowed($" The graph already has an edge connecting nodes {sourseName} and {targetName}. Currently, parallel edges are prohibited in the graph.");
                }
                
                
            }  
        }

        public TNode AddNode(TNode node)
        {
            string nodeName = node.GetName();
            if (nodesMap.ContainsKey(nodeName))
            {
                throw new DublicateException("It is not possible to add the node with same name twice.");
            }
            nodesMap.Add(nodeName, node);

            AdjacencyMap[nodeName] = new();
            ReversedAdjacencyMap[nodeName] = new();

            return node;
        }


        public void DeleteNode(string name)
        {
            if (!nodesMap.TryGetValue(name, out var node))
            {
                throw new NotFoundException("Node", name, "Graph");
            }

            var outgoingEdges = AdjacencyMap[name].Values.ToList();
            for (int i = 0; i < outgoingEdges.Count; i++)
            {
                var edges = outgoingEdges[i];
                for (int j = 0; j < edges.Count; j++)
                {
                    string edge = edges[j].GetName();
                    this.DeleteEdge(edge);
                }
            }

            var incomingEdges = ReversedAdjacencyMap[name].Values.ToList();
            for (int i = 0; i < incomingEdges.Count; i++)
            {
                var edges = incomingEdges[i];
                for (int j = 0; j < edges.Count; j++)
                {
                    string edge = edges[j].GetName();
                    this.DeleteEdge(edge);
                }
            }
            

            nodesMap.Remove(name);
            AdjacencyMap.Remove(name);
            ReversedAdjacencyMap.Remove(name);
        }

        public List<TEdge> GetEdgesBetween(string sourseName, string targetName)
        {
            if (AdjacencyMap.ContainsKey(sourseName))
            {
                if (AdjacencyMap[sourseName].ContainsKey(targetName))
                {
                    List<TEdge> edges = AdjacencyMap[sourseName][targetName];
                    return new List<TEdge>(edges);
                }
            }
            return new List<TEdge>();
        }
        public bool HasNodes()
        {
            return nodesMap.Values.Count > 0;
        }

        public int GetNodeCount()
        {
            return nodesMap.Values.Count;
        }

        public Dictionary<string, Dictionary<string, List<TEdge>>> GetAdjacencyMap()
        {
            return new Dictionary<string, Dictionary<string, List<TEdge>>>(AdjacencyMap);
        }

        
    }
}

