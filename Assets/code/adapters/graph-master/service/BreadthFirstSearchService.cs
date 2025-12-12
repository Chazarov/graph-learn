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
            {
                return result;
            }

            // Получаем карту смежности (для неориентированного графа уже содержит обратные рёбра)
            var adjMap = graph.GetAdjacencyMap();

            // Добавляем начальную вершину в очередь и отмечаем как посещённую
            queue.Enqueue(startNode);
            visitedNodes.Add(startNode.GetName());
            result.Add(startNode);

            while (queue.Count > 0)
            {
                TNode currentNode = queue.Dequeue();
                string nodeName = currentNode.GetName();

                // Проверяем, есть ли исходящие рёбра из текущей вершины
                if (!adjMap.ContainsKey(nodeName))
                {
                    continue;
                }

                // Перебираем всех соседей текущей вершины
                foreach (var targetEntry in adjMap[nodeName])
                {
                    string targetName = targetEntry.Key;
                    List<TEdge> edges = targetEntry.Value;

                    // Берём первое непосещённое ребро к соседу
                    foreach (TEdge edge in edges)
                    {
                        string edgeName = edge.GetName();

                        if (visitedEdges.Contains(edgeName))
                        {
                            continue;
                        }

                        // Добавляем ребро в результат
                        visitedEdges.Add(edgeName);
                        result.Add(edge);

                        // Если целевая вершина ещё не посещена
                        if (!visitedNodes.Contains(targetName))
                        {
                            TNode targetNode = graph.GetNode(targetName);
                            visitedNodes.Add(targetName);
                            result.Add(targetNode);
                            queue.Enqueue(targetNode);
                        }

                        // Для BFS берём только одно ребро к каждому соседу
                        break;
                    }
                }
            }

            return result;
        }
    }
}

