using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace GraphMaster
{
    public class AlghoritmMaster 
    {
        public List<GraphNode> MakeDijkstra(Domain.GraphInterface graph)
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
                // ���������, ��� ��� ������������� �����
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
            // TODO: ���������� ��������� ��������
            return new List<GraphNode>();
        }

        public List<GraphNode> MakeBreadth(Domain.GraphInterface graph)
        {
            if (!graph.HasNodes())
            {
                throw new EmptyGraphException("Breadth-first search");
            }
            // TODO: ���������� ��������� ������ � ������
            return new List<GraphNode>();
        }

        public List<GraphNode> MakeDepth(Domain.GraphInterface graph)
        {
            if (!graph.HasNodes())
            {
                throw new EmptyGraphException("Depth-first search");
            }
            // TODO: ���������� ��������� ������ � �������
            return new List<GraphNode>();
        }

        public List<GraphNode> MakeBF(Domain.GraphInterface graph)
        {
            if (!graph.HasNodes())
            {
                throw new EmptyGraphException("Bellman-Ford algorithm");
            }
            if (!graph.IsWeighed())
            {
                throw new InvalidGraphOperationException("Bellman-Ford algorithm requires a weighted graph");
            }
            // TODO: ���������� ��������� ��������-�����
            return new List<GraphNode>();
        }
    }

}

