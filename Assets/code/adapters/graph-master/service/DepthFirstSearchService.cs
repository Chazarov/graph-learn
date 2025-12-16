using Domain;
using GraphMaster.UnityAdapter.Visualization.Actions;
using System.Collections.Generic;

namespace GraphMaster
{
    public class DepthFirstSearchService<TNode, TEdge> : GraphTraversalServiceInterface<TNode, TEdge>
        where TNode : GraphNodeInterface, GraphPartInterface
        where TEdge : GraphEdgeInterface<TNode>, GraphPartInterface
    {
        public List<ActionInterface> Traverse(GraphInterface<TNode, TEdge> graph)
        {
            List<ActionInterface> actions = new();

            if (!graph.HasNodes())
            {
                return actions;
            }

            TNode root = graph.GetRoot();
            HashSet<string> visitedNodes = new HashSet<string>();
            var adjMap = graph.GetAdjacencyMap();

            DFSRecursive(graph, adjMap, root, visitedNodes, actions);

            return actions;
        }

        private void DFSRecursive(
            GraphInterface<TNode, TEdge> graph,
            Dictionary<string, Dictionary<string, List<TEdge>>> adjMap,
            TNode currentNode,
            HashSet<string> visitedNodes,
            List<ActionInterface> actions)
        {
            string nodeName = currentNode.GetName();

            if (visitedNodes.Contains(nodeName))
            {
                return;
            }

            visitedNodes.Add(nodeName);
            actions.Add(new MarkThis(currentNode));

            if (!adjMap.ContainsKey(nodeName))
            {
                return;
            }

            foreach (var targetEntry in adjMap[nodeName])
            {
                string targetName = targetEntry.Key;
                List<TEdge> edges = targetEntry.Value;

                foreach (TEdge edge in edges)
                {
                    if (visitedNodes.Contains(targetName))
                    {
                        continue;
                    }

                    actions.Add(new MarkThis(edge));

                    TNode targetNode = graph.GetNode(targetName);
                    DFSRecursive(graph, adjMap, targetNode, visitedNodes, actions);

                    break;
                }
            }
        }
    }
}
