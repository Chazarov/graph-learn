using Domain;
using System.Collections.Generic;

namespace GraphMaster
{
    /// <summary>
    /// Сервис для поиска кратчайших путей алгоритмом Дейкстры.
    /// Возвращает словарь с расстояниями от начальной вершины до всех остальных.
    /// </summary>
    public class DijkstraService<TNode, TEdge>
        where TNode : GraphNodeInterface, GraphPartInterface
        where TEdge : GraphEdgeInterface<TNode>, GraphPartInterface
    {
        /// <summary>
        /// Выполняет алгоритм Дейкстры, начиная с корневой вершины.
        /// </summary>
        /// <param name="graph">Граф для обхода</param>
        /// <returns>Словарь: имя вершины -> кратчайшее расстояние до неё</returns>
        public Dictionary<string, float> FindShortestPaths(GraphInterface<TNode, TEdge> graph)
        {
            if (!graph.HasNodes())
            {
                return new Dictionary<string, float>();
            }

            TNode root = graph.GetRoot();
            return FindShortestPaths(graph, root);
        }

        /// <summary>
        /// Выполняет алгоритм Дейкстры, начиная с указанной вершины.
        /// </summary>
        /// <param name="graph">Граф для обхода</param>
        /// <param name="startNode">Начальная вершина</param>
        /// <returns>Словарь: имя вершины -> кратчайшее расстояние до неё</returns>
        public Dictionary<string, float> FindShortestPaths(GraphInterface<TNode, TEdge> graph, TNode startNode)
        {
            Dictionary<string, float> distances = new Dictionary<string, float>();
            HashSet<string> visitedNodes = new HashSet<string>();

            if (!graph.HasNodes() || startNode == null)
            {
                return distances;
            }

            // Получаем карту смежности
            var adjMap = graph.GetAdjacencyMap();

            // Инициализируем расстояния
            foreach (TNode node in graph.GetNodes())
            {
                distances[node.GetName()] = float.MaxValue;
            }

            string startName = startNode.GetName();
            distances[startName] = 0;

            // Основной цикл алгоритма Дейкстры
            while (visitedNodes.Count < graph.GetNodeCount())
            {
                // Находим непосещённую вершину с минимальным расстоянием
                string currentNodeName = GetMinDistanceNode(distances, visitedNodes);
                
                if (currentNodeName == null || distances[currentNodeName] == float.MaxValue)
                {
                    // Все оставшиеся вершины недостижимы
                    break;
                }

                visitedNodes.Add(currentNodeName);

                // Проверяем, есть ли исходящие рёбра из текущей вершины
                if (!adjMap.ContainsKey(currentNodeName))
                {
                    continue;
                }

                // Обновляем расстояния до всех соседей
                foreach (var targetEntry in adjMap[currentNodeName])
                {
                    string targetName = targetEntry.Key;
                    List<TEdge> edges = targetEntry.Value;

                    if (visitedNodes.Contains(targetName))
                    {
                        continue;
                    }

                    // Находим ребро с минимальным весом к этой вершине
                    float minEdgeWeight = float.MaxValue;
                    foreach (TEdge edge in edges)
                    {
                        float weight = edge.GetWeight();
                        if (weight < minEdgeWeight)
                        {
                            minEdgeWeight = weight;
                        }
                    }

                    // Релаксация: обновляем расстояние, если нашли более короткий путь
                    float newDistance = distances[currentNodeName] + minEdgeWeight;
                    if (newDistance < distances[targetName])
                    {
                        distances[targetName] = newDistance;
                    }
                }
            }

            return distances;
        }

        /// <summary>
        /// Находит непосещённую вершину с минимальным расстоянием.
        /// </summary>
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

