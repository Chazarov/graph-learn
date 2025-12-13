using Domain;
using System.Collections.Generic;

namespace GraphMaster
{
    /// <summary>
    /// Сервис для обхода графа в глубину (Depth-First Search).
    /// Обход начинается с указанной вершины, проходит по рёбрам до максимальной глубины,
    /// затем возвращается и исследует другие ветви.
    /// </summary>
    public class DepthFirstSearchService<TNode, TEdge> : GraphTraversalServiceInterface<TNode, TEdge>
        where TNode : GraphNodeInterface, GraphPartInterface
        where TEdge : GraphEdgeInterface<TNode>, GraphPartInterface
    {
        /// <summary>
        /// Выполняет обход графа в глубину, начиная с корневой вершины.
        /// </summary>
        /// <param name="graph">Граф для обхода</param>
        /// <returns>Список элементов графа (узлы и рёбра) в порядке обхода DFS</returns>
        public List<GraphPartInterface> Traverse(GraphInterface<TNode, TEdge> graph)
        {
            if (!graph.HasNodes())
            {
                return new List<GraphPartInterface>();
            }

            TNode root = graph.GetRoot();
            return Traverse(graph, root);
        }

        /// <summary>
        /// Выполняет обход графа в глубину, начиная с указанной вершины.
        /// </summary>
        /// <param name="graph">Граф для обхода</param>
        /// <param name="startNode">Начальная вершина</param>
        /// <returns>Список элементов графа (узлы и рёбра) в порядке обхода DFS</returns>
        public List<GraphPartInterface> Traverse(GraphInterface<TNode, TEdge> graph, TNode startNode)
        {
            List<GraphPartInterface> result = new List<GraphPartInterface>();
            HashSet<string> visitedNodes = new HashSet<string>();
            HashSet<string> visitedEdges = new HashSet<string>();

            if (!graph.HasNodes() || startNode == null)
            {
                return result;
            }

            // Получаем карту смежности (для неориентированного графа уже содержит обратные рёбра)
            var adjMap = graph.GetAdjacencyMap();

            DFSRecursive(graph, adjMap, startNode, visitedNodes, visitedEdges, result);

            return result;
        }

        /// <summary>
        /// Рекурсивный метод для обхода в глубину.
        /// </summary>
        private void DFSRecursive(
            GraphInterface<TNode, TEdge> graph,
            Dictionary<string, Dictionary<string, List<TEdge>>> adjMap,
            TNode currentNode, 
            HashSet<string> visitedNodes, 
            HashSet<string> visitedEdges,
            List<GraphPartInterface> result)
        {
            string nodeName = currentNode.GetName();

            if (visitedNodes.Contains(nodeName))
            {
                return;
            }

            // Добавляем текущую вершину в результат и отмечаем как посещённую
            visitedNodes.Add(nodeName);
            result.Add(currentNode);

            // Проверяем, есть ли исходящие рёбра из текущей вершины
            if (!adjMap.ContainsKey(nodeName))
            {
                return;
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

                    if (visitedNodes.Contains(targetName))
                    {
                        continue;
                    }

                    // Добавляем ребро в результат
                    visitedEdges.Add(edgeName);
                    result.Add(edge);

                    TNode targetNode = graph.GetNode(targetName);
                    DFSRecursive(graph, adjMap, targetNode, visitedNodes, visitedEdges, result);

                    // Для DFS берём только одно ребро к каждому соседу
                    break;
                }
            }
        }
    }
}

