using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GraphMaster
{

    public class GraphNode
    {

        private Graph graph;

        private List<GraphEdge> edges = new List<GraphEdge>();

        private bool hasParralelsEdges = false;

        private int number;

        private string name;

        private string description;


        GraphNode(Graph graph)
        {
            this.graph = graph;
        }

        public void AddEdge(GraphEdge edge)
        {
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


        public List<GraphEdge> GetEdges()
        {
            return edges;
        }

    }

}