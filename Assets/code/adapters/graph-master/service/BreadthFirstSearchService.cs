using Domain;
using System.Collections.Generic;

namespace GraphMaster
{
    /// <summary>
    /// Сервис для обхода графа в ширину (Breadth-First Search).
    /// Обход начинается с указанной вершины, сначала посещаются все соседние вершины,
    /// затем соседи соседей и так далее по уровням.
    /// </summary>
    public class BreadthFirstSearchService<TNode, TEdge> : GraphTraversalServiceInterface<TNode, TEdge>
        where TNode : GraphNodeInterface, GraphPartInterface
        where TEdge : GraphEdgeInterface<TNode>, GraphPartInterface
    {
        public List<GraphPartInterface> Traverse(GraphInterface<TNode, TEdge> graph)
        {
            TNode startNode = graph.GetRoot();
            List<GraphPartInterface> result = new List<GraphPartInterface>();
            HashSet<string> visitedNodes = new HashSet<string>();
            HashSet<string> visitedEdges = new HashSet<string>();
            Queue<TNode> queue = new Queue<TNode>();

            if (!graph.HasNodes() || startNode == null)
                return result;

            var adjMap = graph.GetAdjacencyMap();

            // Старт
            queue.Enqueue(startNode);
            visitedNodes.Add(startNode.GetName());
            result.Add(startNode);

            while (queue.Count > 0)
            {
                TNode currentNode = queue.Dequeue();
                string nodeName = currentNode.GetName();

                if (!adjMap.ContainsKey(nodeName))
                    continue;

                // **КЛЮЧЕВОЕ ИЗМЕНЕНИЕ**: перебираем ВСЕХ соседей!
                foreach (var targetEntry in adjMap[nodeName])
                {
                    string targetName = targetEntry.Key;
                    List<TEdge> edges = targetEntry.Value;

                    // Проверяем непосещённую вершину
                    if (visitedNodes.Contains(targetName))
                        continue;

                    // Берём первое ребро к этой вершине
                    TEdge edge = edges[0];
                    string edgeName = edge.GetName();

                    // Добавляем РЕБРО и ВЕРШИНУ
                    visitedEdges.Add(edgeName);
                    result.Add(edge);      // ← Ребро
                    result.Add(graph.GetNode(targetName)); // ← Вершина

                    visitedNodes.Add(targetName);
                    queue.Enqueue(graph.GetNode(targetName));
                }
            }

            return result;
        }

    }
}

