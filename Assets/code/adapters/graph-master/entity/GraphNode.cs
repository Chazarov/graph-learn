using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Domain;

namespace GraphMaster
{

    public class GraphNode : GraphNodeInterface
    {

        private Graph graph;

        private List<GraphEdgeInterface> edges = new List<GraphEdgeInterface>();

        private bool hasParralelsEdges = false;

        private int number;

        private string name;

        private string description;


        public GraphNode(Graph graph, int number)
        {
            if (graph == null)
            {
                throw new ArgumentNullException(nameof(graph), "Graph cannot be null");
            }
            this.graph = graph;
            this.number = number;
        }

        public GraphNode(Graph graph, int number, string name) : this(graph, number)
        {
            this.name = name;
        }

        public GraphNode(Graph graph, int number, string name, string description) : this(graph, number, name)
        {
            this.description = description;
        }

        public void AddEdge(GraphEdge edge)
        {
            if (edge == null)
            {
                throw new ArgumentNullException(nameof(edge), "Edge cannot be null");
            }


            foreach (GraphEdge childEdge in edges)
            {
                if (childEdge.IsParralel(edge))
                {
                    if (!graph.AllowedParrallelEdges() && graph.IsOriented())
                    {
                        throw new ParralelEdgesNotAllowed();
                    }
                    hasParralelsEdges = true;
                }
            }
            this.edges.Add(edge);
        }

        public void DeleteNode()
        {
            var edgesCopy = new List<GraphEdgeInterface>(edges);
            foreach (GraphEdgeInterface edge in edgesCopy)
            {
                if (edge is GraphEdge graphEdge)
                {
                    this.DisconnectEdge(graphEdge);
                }
            }
            this.edges.Clear();
            this.graph.DeleteNode(this.number);
        }

        public void DisconnectEdge(GraphEdgeInterface edge)
        {
            this.edges.Remove(edge);
        }

        public void DisconnectEdge(GraphEdge edge)
        {
            this.edges.Remove(edge);
        }


        public List<GraphEdgeInterface> GetEdges()
        {
            return new List<GraphEdgeInterface>(edges); 
        }

        public Graph GetGraph()
        {
            return graph;
        }

        public int GetNumber()
        {
            return number;
        }

        public string GetName()
        {
            return name;
        }

        public string GetDescription()
        {
            return description;
        }

        public void SetName(string name)
        {
            this.name = name;
        }

        public void SetDescription(string description)
        {
            this.description = description;
        }

        List<GraphEdgeInterface> GraphNodeInterface.GetEdges()
        {
            throw new NotImplementedException();
        }

        GraphInterface GraphNodeInterface.GetGraph()
        {
            throw new NotImplementedException();
        }

        public void AddEdge(GraphEdgeInterface edge)
        {
            throw new NotImplementedException();
        }
    }

}