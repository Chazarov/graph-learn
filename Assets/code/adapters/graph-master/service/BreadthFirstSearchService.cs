using Domain;
using GraphMaster.Visualization.Actions;
using System.Collections.Generic;

namespace GraphMaster
{
    public class BreadthFirstSearchService<TNode, TEdge> : GraphTraversalServiceInterface<TNode, TEdge>
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

            TNode startNode = graph.GetRoot();
            HashSet<string> visitedNodes = new HashSet<string>();
            Queue<TNode> queue = new Queue<TNode>();

            var adjMap = graph.GetAdjacencyMap();

            queue.Enqueue(startNode);
            visitedNodes.Add(startNode.GetName());
            actions.Add(new MarkThis(startNode));

            while (queue.Count > 0)
            {
                TNode currentNode = queue.Dequeue();
                string nodeName = currentNode.GetName();

                if (!adjMap.ContainsKey(nodeName))
                    continue;

                foreach (var targetEntry in adjMap[nodeName])
                {
                    string targetName = targetEntry.Key;
                    List<TEdge> edges = targetEntry.Value;

                    if (visitedNodes.Contains(targetName))
                        continue;

                    TEdge edge = edges[0];
                    actions.Add(new MarkThis(edge));
                    actions.Add(new MarkThis(graph.GetNode(targetName)));

                    visitedNodes.Add(targetName);
                    queue.Enqueue(graph.GetNode(targetName));
                }
            }

            return actions;
        }
    }
}
