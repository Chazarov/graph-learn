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

        // Simple undirected grpah for a simple calculations
        private Dictionary<string, HashSet<string>> uAdjacencyMap = new();
        // Algorithms with a directed weighed graph
        private Dictionary<string, Dictionary<string, List<(float weight, string edgeName)>>> dWAdjacencyMap = new();

        private bool parralelEdgesAreAllowed = false;
        private bool loopsAreAllowed = false;
        private bool isDirected = false;

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


            if (!dWAdjacencyMap[sourseName].ContainsKey(targetName)) dWAdjacencyMap[sourseName][targetName] = new();
            dWAdjacencyMap[sourseName][targetName].Add((edgeWeight, edgeName));

            uAdjacencyMap[sourseName].Add(targetName);
            uAdjacencyMap[targetName].Add(sourseName);


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

            dWAdjacencyMap[sourceName][targetName].Remove((edgeWeight, edgeName));
            if(dWAdjacencyMap[sourceName][targetName].Count == 0)
            {
                dWAdjacencyMap[sourceName].Remove(targetName);

                if (!dWAdjacencyMap[targetName].ContainsKey(sourceName))
                {
                    uAdjacencyMap[sourceName].Remove(targetName);
                    uAdjacencyMap[targetName].Remove(sourceName);
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

            if (!this.parralelEdgesAreAllowed)
            {
                if (uAdjacencyMap[sourseName].Contains(targetName))
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

            dWAdjacencyMap[nodeName] = new();
            uAdjacencyMap[nodeName] = new();

            return node;
        }


        public void DeleteNode(string name)
        {
            if (!nodesMap.TryGetValue(name, out var node))
            {
                throw new NotFoundException("Node", name, "Graph");
            }

            var pairedNodes = dWAdjacencyMap[name].Values.ToList();
            for (int i = 0; i < pairedNodes.Count; i++)
            {
                var edges = pairedNodes[i];
                for (int j = 0; j < edges.Count; j++)
                {
                    string edge = edges[j].edgeName;
                    this.DeleteEdge(edge);
                }
            }

            foreach (var nodeEntry in dWAdjacencyMap)
            {
                if (nodeEntry.Key == name) continue;

                if (nodeEntry.Value.ContainsKey(name))
                {
                    var incomingEdges = nodeEntry.Value[name].ToList();
                    foreach (var edgeInfo in incomingEdges)
                    {
                        this.DeleteEdge(edgeInfo.edgeName);
                    }
                }
            }

            nodesMap.Remove(name);
            dWAdjacencyMap.Remove(name);
            uAdjacencyMap.Remove(name);

            foreach (var nodeEntry in uAdjacencyMap)
            {
                nodeEntry.Value.Remove(name);
            }
        }



        public bool HasNodes()
        {
            return nodesMap.Values.Count > 0;
        }

        public int GetNodeCount()
        {
            return nodesMap.Values.Count;
        }

        public Dictionary<string, Dictionary<string, List<(float weight, string edgeName)>>> GetAdjacencyMap()
        {
            return new Dictionary<string, Dictionary<string, List<(float weight, string edgeName)>>>(dWAdjacencyMap);
        }


        


    }
}

