using GraphMaster;
using System.Collections.Generic;

namespace Domain
{
    /// <summary>
    /// Интерфейс для сервисов обхода графа.
    /// </summary>
    public interface GraphTraversalServiceInterface<TNode, TEdge> 
        where TNode : GraphNodeInterface, GraphPartInterface 
        where TEdge : GraphEdgeInterface<TNode>, GraphPartInterface
    {
        /// <summary>
        /// Выполняет обход графа, начиная с корневой вершины.
        /// </summary>
        /// <param name="graph">Граф для обхода</param>
        /// <returns>Список элементов графа (узлы и рёбра) в порядке обхода</returns>
        List<GraphPartInterface> Traverse(GraphInterface<TNode, TEdge> graph);

        /// <summary>
        /// Выполняет обход графа, начиная с указанной вершины.
        /// </summary>
        /// <param name="graph">Граф для обхода</param>
        /// <param name="startNode">Начальная вершина</param>
        /// <returns>Список элементов графа (узлы и рёбра) в порядке обхода</returns>
        List<GraphPartInterface> Traverse(GraphInterface<TNode, TEdge> graph, TNode startNode);
    }
}

