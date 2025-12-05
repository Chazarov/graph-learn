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


        // Algorithms with an undirected graph
        private Dictionary<string, List<string>> nodesEdgesMap = new();

        // Algoritms with a weighed graph 
        public Dictionary<string, List<(string to, float weight, string edgeName)>> adjacencyMap = new();

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

            if (!nodesEdgesMap[sourseName].Contains(edgeName)) nodesEdgesMap[sourseName] = new List<string>();
            if (!nodesEdgesMap[targetName].Contains(edgeName)) nodesEdgesMap[targetName] = new List<string>();
            nodesEdgesMap[sourseName].Add(edgeName);
            nodesEdgesMap[targetName].Add(edgeName);


            if (!adjacencyMap[sourseName].Contains((targetName, edgeWeight, edgeName))) adjacencyMap[sourseName] = new List<(string, float, string)>();
            adjacencyMap[sourseName].Add((targetName, edgeWeight, edgeName));

            return edge;
        }
        public void DeleteEdge(string name)
        {
            var edge = this.GetEdge(name);
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


            nodesEdgesMap[targetName].Remove(edgeName);
            nodesEdgesMap[sourceName].Remove(edgeName);


            adjacencyMap[sourceName].Remove((targetName, edgeWeight, edgeName));
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
                bool occuerence =  adjacencyMap[sourseName].FindAll(item => item.to == targetName).Count != 0;
                if (occuerence)
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

            nodesEdgesMap[nodeName] = new();

            adjacencyMap[nodeName] = new();



            return node;
        }


        public void DeleteNode(string name)
        {
            if (!nodesMap.TryGetValue(name, out var node))
            {
                throw new NotFoundException("Node", name, "Graph");
            }

            

            nodesMap.Remove(name);

            var edgesToDelete = nodesEdgesMap[name];
            for(int i = 0; i < edgesToDelete.Count; i++)
            {
                string edge = edgesToDelete[i];
                this.DeleteEdge(edge);

            }

            nodesEdgesMap.Remove(name);


            adjacencyMap.Remove(name);
        }

        public bool HasNodes()
        {
            return nodesMap.Values.Count > 0;
        }

        public int GetNodeCount()
        {
            return nodesMap.Values.Count;
        }

        public Dictionary<string, List<(string to, float weight, string edgeName)>> GetAdjacencyMap()
        {
            return new Dictionary<string, List<(string to, float weight, string edgeName)>>(adjacencyMap);
        }


        


    }
}

