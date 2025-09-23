using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using domain;
using System;
using System.Linq;

namespace GraphMaster
{
    public class Graph : domain.GraphInterface
    {
        private bool isOriented = false;
        private bool isWeighed = false;
        private bool allowParallelEdges = false;
        private bool allowNegativeEdges = false;
        private bool allowLoops = false;
        
        private List<GraphNode> nodes = new List<GraphNode>();

        public bool AllowedParrallelEdges()
        {
            return allowParallelEdges;
        }

        public bool AllowedNegativeEdges()
        {
            return allowNegativeEdges;
        }

        public bool AllowedLoops()
        {
            return allowLoops;
        }

        public bool IsOriented()
        {
            return isOriented;
        }

        public bool IsWeighed()
        {
            return isWeighed;
        }

        public void MakeOriented()
        {
            isOriented = true;
        }

        public void MakeParralel()
        {
            allowParallelEdges = true;
        }

        public void MakeWeighed()
        {
            isWeighed = true;
        }

        public void AllowNegative()
        {
            allowNegativeEdges = true;
        }

        public void AllowLoops()
        {
            allowLoops = true;
        }

        // Методы для работы с вершинами
        public GraphNode AddNode(int number)
        {
            if (nodes.Any(n => n.GetNumber() == number))
            {
                throw new DuplicateNodeException(number);
            }
            
            var node = new GraphNode(this, number);
            nodes.Add(node);
            return node;
        }

        public void DeleteNode(int nodeNumber)
        {
            var node = nodes.FirstOrDefault(n => n.GetNumber() == nodeNumber);
            if (node == null)
            {
                throw new NodeNotFoundException(nodeNumber);
            }
            this.nodes.Remove(node);
        }

        public GraphNode AddNode(int number, string name)
        {
            if (nodes.Any(n => n.GetNumber() == number))
            {
                throw new DuplicateNodeException(number);
            }
            
            var node = new GraphNode(this, number, name);
            nodes.Add(node);
            return node;
        }

        public GraphNode AddNode(int number, string name, string description)
        {
            if (nodes.Any(n => n.GetNumber() == number))
            {
                throw new DuplicateNodeException(number);
            }
            
            var node = new GraphNode(this, number, name, description);
            nodes.Add(node);
            return node;
        }

        public GraphNode GetNode(int number)
        {
            var node = nodes.FirstOrDefault(n => n.GetNumber() == number);
            if (node == null)
            {
                throw new NodeNotFoundException(number);
            }
            return node;
        }

        public List<GraphNode> GetNodes()
        {
            return new List<GraphNode>(nodes);
        }

        public bool HasNodes()
        {
            return nodes.Count > 0;
        }

        public int GetNodeCount()
        {
            return nodes.Count;
        }

        // Методы для алгоритмов (с проверкой на пустой граф)
        public List<GraphNode> MakeDijkstra()
        {
            if (!HasNodes())
            {
                throw new EmptyGraphException("Dijkstra algorithm");
            }
            if (!isWeighed)
            {
                throw new InvalidGraphOperationException("Dijkstra algorithm requires a weighted graph");
            }
            if (!allowNegativeEdges)
            {
                // Проверяем, что нет отрицательных ребер
                foreach (var node in nodes)
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

        public List<GraphNode> MakeBreadth()
        {
            if (!HasNodes())
            {
                throw new EmptyGraphException("Breadth-first search");
            }
            // TODO: Реализация алгоритма обхода в ширину
            return new List<GraphNode>();
        }

        public List<GraphNode> MakeDepth()
        {
            if (!HasNodes())
            {
                throw new EmptyGraphException("Depth-first search");
            }
            // TODO: Реализация алгоритма обхода в глубину
            return new List<GraphNode>();
        }

        public List<GraphNode> MakeBF()
        {
            if (!HasNodes())
            {
                throw new EmptyGraphException("Bellman-Ford algorithm");
            }
            if (!isWeighed)
            {
                throw new InvalidGraphOperationException("Bellman-Ford algorithm requires a weighted graph");
            }
            // TODO: Реализация алгоритма Беллмана-Форда
            return new List<GraphNode>();
        }
    }

}

