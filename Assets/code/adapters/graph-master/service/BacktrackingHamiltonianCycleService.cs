using Domain;
using GraphMaster.Visualization.Actions;
using System.Collections.Generic;

namespace GraphMaster
{
    public class BacktrackingHamiltonianCycleService<TNode, TEdge>
        where TNode : GraphNodeInterface, GraphPartInterface
        where TEdge : GraphEdgeInterface<TNode>, GraphPartInterface
    {
        private HamiltonianCheckService<TNode, TEdge> hamiltonianCheck = new();

        public List<ActionInterface> FindHamiltonianPath(GraphInterface<TNode, TEdge> graph)
        {
            List<ActionInterface> actions = new();

            hamiltonianCheck.CheckHamiltonian(graph);

            var adjMap = graph.GetAdjacencyMap();
            TNode startNode = graph.GetRoot();
            HashSet<string> visited = new();
            List<TNode> path = new();
            List<TEdge> pathEdges = new();

            actions.Add(new MarkThis(startNode));
            actions.Add(new SetAdditionalValue("1", startNode));
            Backtrack(graph, adjMap, startNode, visited, path, pathEdges, actions);

            return actions;
        }

        private bool Backtrack(
            GraphInterface<TNode, TEdge> graph,
            Dictionary<string, Dictionary<string, List<TEdge>>> adjMap,
            TNode currentNode,
            HashSet<string> visited,
            List<TNode> path,
            List<TEdge> pathEdges,
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
                actions.Add(new UnmarkThisFast(currentNode));
                actions.Add(new HideAdditionalValueFast(currentNode));
                if (pathEdges.Count > 0)
                {
                    TEdge lastEdge = pathEdges[pathEdges.Count - 1];
                    actions.Add(new UnmarkThisFast(lastEdge));
                    pathEdges.RemoveAt(pathEdges.Count - 1);
                }
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

                    pathEdges.Add(edge);
                    actions.Add(new MarkThis(edge));
                    actions.Add(new MarkThis(targetNode));
                    actions.Add(new SetAdditionalValue((path.Count + 1).ToString(), targetNode));

                    if (Backtrack(graph, adjMap, targetNode, visited, path, pathEdges, actions))
                    {
                        return true;
                    }
                }
            }

            visited.Remove(nodeName);
            path.RemoveAt(path.Count - 1);
            actions.Add(new UnmarkThisFast(currentNode));
            actions.Add(new HideAdditionalValueFast(currentNode));
            if (pathEdges.Count > 0)
            {
                TEdge lastEdge = pathEdges[pathEdges.Count - 1];
                actions.Add(new UnmarkThisFast(lastEdge));
                pathEdges.RemoveAt(pathEdges.Count - 1);
            }

            return false;
        }
    }
}
