using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace GraphMaster
{
    public class AlghoritmMaster 
    {
        public List<GraphNode> MakeDijkstra(Domain.IMutableGraph graph)
        {
            if (!graph.HasNodes())
            {
                throw new EmptyGraphException("Dijkstra algorithm");
            }
            if (!graph.IsWeighed())
            {
                throw new InvalidGraphOperationException("Dijkstra algorithm requires a weighted graph");
            }
            if (!graph.AllowedNegativeEdges())
            {
                // Проверяем, что нет отрицательных ребер
                foreach (var node in graph.GetNodes())
                {
                    foreach (var edge in node.GetEdges())
                    {
                        if (edge.HasWeight() && edge.GetWeight() < 0)
                        {
                            throw new InvalidGraphOperationException("Dijkstra algorithm cannot work with negative edges");
                        }
                    }
                }
            }
            // TODO: Реализация алгоритма Дейкстры
            return new List<GraphNode>();
        }

        public List<GraphNode> MakeBreadth(Domain.IMutableGraph graph)
        {
            if (!graph.HasNodes())
            {
                throw new EmptyGraphException("Breadth-first search");
            }
            // TODO: Реализация алгоритма обхода в ширину
            return new List<GraphNode>();
        }

        public List<GraphNode> MakeDepth(Domain.IMutableGraph graph)
        {
            if (!graph.HasNodes())
            {
                throw new EmptyGraphException("Depth-first search");
            }
            // TODO: Реализация алгоритма обхода в глубину
            return new List<GraphNode>();
        }

        public List<GraphNode> MakeBF(Domain.IMutableGraph graph)
        {
            if (!graph.HasNodes())
            {
                throw new EmptyGraphException("Bellman-Ford algorithm");
            }
            if (!graph.IsWeighed())
            {
                throw new InvalidGraphOperationException("Bellman-Ford algorithm requires a weighted graph");
            }
            // TODO: Реализация алгоритма Беллмана-Форда
            return new List<GraphNode>();
        }
    }

}

