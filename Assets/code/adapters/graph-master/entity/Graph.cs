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

        private TNode root;

        // Algorithms with a directed weighed graph
        private Dictionary<string, Dictionary<string, List<TEdge>>> AdjacencyMap = new();

        private Dictionary<string, Dictionary<string, List<TEdge>>> ReversedAdjacencyMap = new();

        private bool parralelEdgesAreAllowed = false;
        private bool loopsAreAllowed = false;
        private bool isDirected = false;

        public void SetParralelEdgesAllowed(bool value)
        {
            if (!value)
            {
                bool hasParralel = HasDirectedParallelEdges();

                if (!isDirected)
                {
                    hasParralel = hasParralel || HasParallelEdges();
                }
                if (hasParralel)
                {
                    throw new ImpossibleToSetGraphParralel("It is impossible to change the graph type to a non-parallel one. There are parallel edges in the graph");
                }
            }
            
            parralelEdgesAreAllowed = value;
        }

        public void SetDirected(bool value)
        {
            if (parralelEdgesAreAllowed && !value)
            {
                bool hasParallel = HasParallelEdges();
                if (hasParallel)
                {
                    throw new ImpossibleToSetGraphDirection("It is impossible to change the graph type to a non-directed one. There are parallel edges in the graph");
                }
            }
            
            isDirected = value;
        }


        public TEdge AddEdge(TEdge edge)
        {
            string sourseName = edge.GetSourseNode().GetName();
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
            string sourceName = edge.GetSourseNode().GetName();
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
                if (AdjacencyMap[sourseName].ContainsKey(targetName))
                {
                    throw new ParralelEdgesNotAllowed($" The graph already has an edge connecting nodes {sourseName} and {targetName}. Currently, parallel edges are prohibited in the graph.");
                }

                if (!isDirected)
                {
                    if (ReversedAdjacencyMap[sourseName].ContainsKey(targetName))
                    {
                        throw new ParralelEdgesNotAllowed($" The graph already has an edge connecting nodes {sourseName} and {targetName}. Currently, parallel edges are prohibited in the graph.");
                    }
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

            if (this.nodesMap.Count == 1)
            {
                this.SetRoot(node);
            }



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

        public List<TEdge> GetEdgesBetween(string node1, string node2)
        {
            if (AdjacencyMap.ContainsKey(node1))
            {
                List<TEdge> edges = new();
                if (AdjacencyMap[node1].ContainsKey(node2))
                {
                    edges.AddRange(AdjacencyMap[node1][node2]);
                    
                }
                if (AdjacencyMap[node2].ContainsKey(node1))
                {
                    edges.AddRange(AdjacencyMap[node2][node1]);

                }
                return new List<TEdge>(edges);
            }
            return new List<TEdge>();
        }

        public bool HasDirectedParallelEdges()
        {
            foreach (var sourceEntry in AdjacencyMap)
            {
                foreach (var targetEntry in sourceEntry.Value)
                {
                    if (targetEntry.Value.Count > 1)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool HasParallelEdges()
        {
            HashSet<string> checkedPairs = new HashSet<string>();

            foreach (var sourceEntry in AdjacencyMap)
            {
                string sourceName = sourceEntry.Key;

                foreach (var targetEntry in sourceEntry.Value)
                {
                    string targetName = targetEntry.Key;

                    string pairKey = string.Compare(sourceName, targetName) < 0
                        ? $"{sourceName}_{targetName}"
                        : $"{targetName}_{sourceName}";

                    if (checkedPairs.Contains(pairKey))
                    {
                        continue;
                    }
                    checkedPairs.Add(pairKey);

                    // Считаем рёбра в обоих направлениях
                    int edgeCount = targetEntry.Value.Count;

                    if (AdjacencyMap.ContainsKey(targetName) && AdjacencyMap[targetName].ContainsKey(sourceName))
                    {
                        edgeCount += AdjacencyMap[targetName][sourceName].Count;
                    }

                    if (edgeCount > 1)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public Dictionary<string, Dictionary<string, List<TEdge>>> GetAdjacencyMap()
        {
            var adj = new Dictionary<string, Dictionary<string, List<TEdge>>>();
            foreach (var sourceEntry in AdjacencyMap)
            {
                adj[sourceEntry.Key] = new Dictionary<string, List<TEdge>>();
                foreach (var targetEntry in sourceEntry.Value)
                {
                    adj[sourceEntry.Key][targetEntry.Key] = new List<TEdge>(targetEntry.Value);
                }
            }
            
            if (!isDirected)
            {
                foreach (var sourceEntry in ReversedAdjacencyMap)
                {
                    string sN = sourceEntry.Key;
                    foreach (var targetEntry in sourceEntry.Value)
                    {
                        string tN = targetEntry.Key;
                        if (adj[sN].ContainsKey(tN))
                        {
                            adj[sN][tN].AddRange(ReversedAdjacencyMap[sN][tN]);
                        }
                        else
                        {
                            adj[sN][tN] = new List<TEdge>();
                            adj[sN][tN].AddRange(ReversedAdjacencyMap[sN][tN]);
                        }
                    }
                }
            }

            return adj;
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

        public bool HasNodes()
        {
            return nodesMap.Values.Count > 0;
        }

        public int GetNodeCount()
        {
            return nodesMap.Values.Count;
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

        public TNode GetRoot()
        {
            return this.root;
        }

        public void SetRoot(TNode root)
        {
            try
            {
                GetNode(root.GetName());
            }
            catch (NotFoundException) 
            {
                throw new InvalidGraphOperationException("It is not possible to add a new root. The node does not belong to this graph.");
            } 
            
            this.root = root;
        }

        /// <summary>
        /// Возвращает AdjacencyMap в виде отформатированной JSON-строки.
        /// </summary>
        public string GetAdjacencyMapAsJson()
        {
            return AdjacencyMapToJson(AdjacencyMap);
        }

        /// <summary>
        /// Возвращает ReversedAdjacencyMap в виде отформатированной JSON-строки.
        /// </summary>
        public string GetReversedAdjacencyMapAsJson()
        {
            return AdjacencyMapToJson(ReversedAdjacencyMap);
        }

        private string AdjacencyMapToJson(Dictionary<string, Dictionary<string, List<TEdge>>> map)
        {
            var sb = new System.Text.StringBuilder();
            sb.AppendLine("{");

            var sourceKeys = map.Keys.ToList();
            for (int i = 0; i < sourceKeys.Count; i++)
            {
                string sourceNode = sourceKeys[i];
                sb.AppendLine($"  \"{sourceNode}\": {{");

                var targetKeys = map[sourceNode].Keys.ToList();
                for (int j = 0; j < targetKeys.Count; j++)
                {
                    string targetNode = targetKeys[j];
                    var edges = map[sourceNode][targetNode];
                    var edgeNames = edges.Select(e => $"\"{e.GetName()}\"");

                    sb.Append($"    \"{targetNode}\": [{string.Join(", ", edgeNames)}]");
                    sb.AppendLine(j < targetKeys.Count - 1 ? "," : "");
                }

                sb.Append("  }");
                sb.AppendLine(i < sourceKeys.Count - 1 ? "," : "");
            }

            sb.AppendLine("}");
            return sb.ToString();
        }

    }
}

