using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Domain;

namespace GraphMaster
{

    public class GraphNode : GraphNodeInterface
    {
        private List<GraphEdgeInterface> edges = new List<GraphEdgeInterface>();

        private bool hasParralelsEdges = false;


        private string name;

        private string description;



        public GraphNode(string name)
        {
            this.name = name;
        }

        public GraphNode(string name, string description) : this(name)
        {
            this.description = description;
        }

        public void AddEdge(GraphEdgeInterface edge)
        {
            if (edge == null)
            {
                throw new ArgumentNullException(nameof(edge), "Edge cannot be null");
            }

            this.edges.Add(edge);
        }

        public void DisconnectEdge(GraphEdgeInterface edge)
        {
            this.edges.Remove(edge);
        }


        public List<GraphEdgeInterface> GetEdges()
        {
            return new List<GraphEdgeInterface>(edges); 
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

        
        public bool HasParrallelEdges()
        {
            return this.hasParralelsEdges;
        }
    }

}