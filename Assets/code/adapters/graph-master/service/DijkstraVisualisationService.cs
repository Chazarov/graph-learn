using Domain;
using GraphMaster.UnityAdapter.Visualization.Actions;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace GraphMaster
{
    public class DijkstraVisualisationService<TNode, TEdge>
        where TNode : GraphNodeInterface, GraphPartInterface, GraphObjectUiActionsInterface
        where TEdge : GraphEdgeInterface<TNode>, GraphPartInterface, GraphObjectUiActionsInterface
    {
        public List<ActionInterface> MakeDijkstra(GraphInterface<TNode, TEdge> graph)
        {
            if (!graph.HasNodes())
            {
                return new();
            }

            TNode root = graph.GetRoot();
            return FindShortestPaths(graph, root);
        }

        private List<ActionInterface> FindShortestPaths(GraphInterface<TNode, TEdge> graph, TNode startNode)
        {
            Dictionary<string, float> distances = new Dictionary<string, float>();
            HashSet<string> visitedNodes = new HashSet<string>();
            List<ActionInterface> actions = new();

            var adjMap = graph.GetAdjacencyMap();

            string startName = startNode.GetName();
            distances[startName] = 0;

            foreach (TNode node in graph.GetNodes())
            {
                if(node.GetName() != startName)
                {
                    actions.Add(new SetAdditionalValueFast("inf", node));
                    distances[node.GetName()] = float.MaxValue;
                }
            }

            actions.Add(new SetAdditionalValueFast("0", startNode));

            

            while (visitedNodes.Count < graph.GetNodeCount())
            {
                string currentNodeName = GetMinDistanceNode(distances, visitedNodes);
                
                if (currentNodeName == null || distances[currentNodeName] == float.MaxValue)
                {
                    break;
                }

                visitedNodes.Add(currentNodeName);

                if (!adjMap.ContainsKey(currentNodeName))
                {
                    continue;
                }


                foreach (var targetEntry in adjMap[currentNodeName])
                {
                    string targetName = targetEntry.Key;
                    List<TEdge> edges = targetEntry.Value;

                    if (visitedNodes.Contains(targetName))
                    {
                        continue;
                    }

                    float minEdgeWeight = float.MaxValue;
                    bool minEdgeSelected = false;
                    int minEdge = 0;
                    for(var i = 0; i < edges.Count; i++)
                    {
                        var edge = edges[i];
                        float weight = edge.GetWeight();
                        if (weight < minEdgeWeight)
                        {
                            minEdgeWeight = weight;
                            minEdge = i;
                            minEdgeSelected = true;
                        }
                    }
                    if (minEdgeSelected)
                    {
                        actions.Add(new MarkThis(edges[minEdge]));
                    }

                    if (visitedNodes.Contains(targetName))
                    {
                        continue;
                    }

                    float newDistance = distances[currentNodeName] + minEdgeWeight;
                    if (newDistance < distances[targetName])
                    {
                        distances[targetName] = newDistance;
                        actions.Add(new SetAdditionalValue(newDistance.ToString(), graph.GetNode(targetName)));
                    }
                }
            }


            return actions;
        }

        private string GetMinDistanceNode(Dictionary<string, float> distances, HashSet<string> visitedNodes)
        {
            float minDistance = float.MaxValue;
            string minNode = null;

            foreach (var pair in distances)
            {
                if (!visitedNodes.Contains(pair.Key) && pair.Value < minDistance)
                {
                    minDistance = pair.Value;
                    minNode = pair.Key;
                }
            }

            return minNode;
        }
    }
}
