using Domain;
using GraphMaster.Visualization.Actions;
using System.Collections.Generic;

namespace GraphMaster
{
    public class BacktrackingService<TNode, TEdge>
        where TNode : GraphNodeInterface, GraphPartInterface
        where TEdge : GraphEdgeInterface<TNode>, GraphPartInterface
    {
        public List<ActionInterface> FindHamiltonianPath(GraphInterface<TNode, TEdge> graph)
        {
            List<ActionInterface> actions = new();

            if (!graph.HasNodes())
                return actions;

            var adjMap = graph.GetAdjacencyMap();
            TNode startNode = graph.GetRoot();
            HashSet<string> visited = new();
            List<TNode> path = new();

            actions.Add(new MarkThis(startNode));
            Backtrack(graph, adjMap, startNode, visited, path, actions);

            return actions;
        }

        private bool Backtrack(
            GraphInterface<TNode, TEdge> graph,
            Dictionary<string, Dictionary<string, List<TEdge>>> adjMap,
            TNode currentNode,
            HashSet<string> visited,
            List<TNode> path,
            List<ActionInterface> actions)
        {
            string nodeName = currentNode.GetName();
            visited.Add(nodeName);
            path.Add(currentNode);

            if (path.Count == graph.GetNodeCount())
            {
                return true;
            }

            if (!adjMap.ContainsKey(nodeName))
            {
                visited.Remove(nodeName);
                path.RemoveAt(path.Count - 1);
                actions.Add(new SetAdditionalValue("X", currentNode));
                return false;
            }

            foreach (var targetEntry in adjMap[nodeName])
            {
                string targetName = targetEntry.Key;
                List<TEdge> edges = targetEntry.Value;

                if (visited.Contains(targetName))
                    continue;

                if (edges.Count > 0)
                {
                    TEdge edge = edges[0];
                    TNode targetNode = graph.GetNode(targetName);

                    actions.Add(new MarkThis(edge));
                    actions.Add(new MarkThis(targetNode));

                    if (Backtrack(graph, adjMap, targetNode, visited, path, actions))
                    {
                        return true;
                    }
                }
            }

            visited.Remove(nodeName);
            path.RemoveAt(path.Count - 1);
            actions.Add(new SetAdditionalValue("X", currentNode));

            return false;
        }
    }
}

