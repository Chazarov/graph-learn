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

            DFSRecursive(graph, startNode, visitedNodes, visitedEdges, result);

            return result;
        }

        /// <summary>
        /// Рекурсивный метод для обхода в глубину.
        /// </summary>
        private void DFSRecursive(
            GraphInterface<TNode, TEdge> graph, 
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

            // Получаем все рёбра графа и ищем исходящие из текущей вершины
            List<TEdge> edges = graph.GetEdges();

            foreach (TEdge edge in edges)
            {
                string edgeName = edge.GetName();
                TNode sourceNode = edge.GetSourceNode();
                TNode targetNode = edge.GetTargetNode();

                // Проверяем, является ли текущая вершина источником ребра
                if (sourceNode.GetName() == nodeName && !visitedEdges.Contains(edgeName))
                {
                    // Добавляем ребро в результат
                    visitedEdges.Add(edgeName);
                    result.Add(edge);

                    // Рекурсивно обходим целевую вершину
                    DFSRecursive(graph, targetNode, visitedNodes, visitedEdges, result);
                }
                // Для неориентированного графа также проверяем обратное направление
                else if (targetNode.GetName() == nodeName && !visitedEdges.Contains(edgeName))
                {
                    if (!visitedNodes.Contains(sourceNode.GetName()))
                    {
                        // Добавляем ребро в результат
                        visitedEdges.Add(edgeName);
                        result.Add(edge);

                        // Рекурсивно обходим исходную вершину
                        DFSRecursive(graph, sourceNode, visitedNodes, visitedEdges, result);
                    }
                }
            }
        }
    }
}

