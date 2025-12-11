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
        /// <summary>
        /// Выполняет обход графа в ширину, начиная с корневой вершины.
        /// </summary>
        /// <param name="graph">Граф для обхода</param>
        /// <returns>Список элементов графа (узлы и рёбра) в порядке обхода BFS</returns>
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
        /// Выполняет обход графа в ширину, начиная с указанной вершины.
        /// </summary>
        /// <param name="graph">Граф для обхода</param>
        /// <param name="startNode">Начальная вершина</param>
        /// <returns>Список элементов графа (узлы и рёбра) в порядке обхода BFS</returns>
        public List<GraphPartInterface> Traverse(GraphInterface<TNode, TEdge> graph, TNode startNode)
        {
            List<GraphPartInterface> result = new List<GraphPartInterface>();
            HashSet<string> visitedNodes = new HashSet<string>();
            HashSet<string> visitedEdges = new HashSet<string>();
            Queue<TNode> queue = new Queue<TNode>();

            if (!graph.HasNodes() || startNode == null)
            {
                return result;
            }

            // Добавляем начальную вершину в очередь и отмечаем как посещённую
            queue.Enqueue(startNode);
            visitedNodes.Add(startNode.GetName());
            result.Add(startNode);

            // Получаем все рёбра графа
            List<TEdge> allEdges = graph.GetEdges();

            while (queue.Count > 0)
            {
                TNode currentNode = queue.Dequeue();
                string nodeName = currentNode.GetName();

                // Ищем все рёбра, связанные с текущей вершиной
                foreach (TEdge edge in allEdges)
                {
                    string edgeName = edge.GetName();
                    TNode sourceNode = edge.GetSourceNode();
                    TNode targetNode = edge.GetTargetNode();

                    // Проверяем, является ли текущая вершина источником ребра
                    if (sourceNode.GetName() == nodeName && !visitedEdges.Contains(edgeName))
                    {
                        string targetName = targetNode.GetName();

                        if (!visitedNodes.Contains(targetName))
                        {
                            // Добавляем ребро в результат
                            visitedEdges.Add(edgeName);
                            result.Add(edge);

                            // Добавляем целевую вершину в очередь и результат
                            visitedNodes.Add(targetName);
                            result.Add(targetNode);
                            queue.Enqueue(targetNode);
                        }
                    }
                    // Для неориентированного графа также проверяем обратное направление
                    else if (targetNode.GetName() == nodeName && !visitedEdges.Contains(edgeName))
                    {
                        string sourceName = sourceNode.GetName();

                        if (!visitedNodes.Contains(sourceName))
                        {
                            // Добавляем ребро в результат
                            visitedEdges.Add(edgeName);
                            result.Add(edge);

                            // Добавляем исходную вершину в очередь и результат
                            visitedNodes.Add(sourceName);
                            result.Add(sourceNode);
                            queue.Enqueue(sourceNode);
                        }
                    }
                }
            }

            return result;
        }
    }
}

