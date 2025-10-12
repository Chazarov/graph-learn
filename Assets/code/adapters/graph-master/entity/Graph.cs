using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Domain;
using System;
using System.Linq;
using Unity.VisualScripting;
using System.Xml.Linq;

namespace GraphMaster
{
    public class Graph : Domain.GraphInterface
    {
        private bool isOriented = false;
        private bool isWeighed = false;
        private bool allowParallelEdges = false;
        private bool allowNegativeEdges = false;
        private bool allowLoops = false;

        private List<GraphNodeInterface> nodes = new List<GraphNodeInterface>();

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

        public void AddNode(GraphNodeInterface node)
        {
            if (nodes.Any(n => n == node))
            {
                throw new DuplicateNodeException(node.ToString());
            }

            nodes.Add(node);
        }

        public GraphNode AddNode(string name, string description)
        {
            var node = new GraphNode(this, name, description);
            nodes.Add(node);
            return node;
        }

        public GraphNode AddNode(string name)
        {
            var node = new GraphNode(this, name);
            nodes.Add(node);
            return node;
        }
        
        public GraphNode AddNode()
        {
            var node = new GraphNode(this);
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

        public GraphNodeInterface GetNode(int number)
        {
            var node = nodes.FirstOrDefault(n => n.GetNumber() == number);
            if (node == null)
            {
                throw new NodeNotFoundException(number);
            }
            return node;
        }

        public List<GraphNodeInterface> GetNodes()
        {
            return new List<GraphNodeInterface>(this.nodes);
        }

        public bool HasNodes()
        {
            return nodes.Count > 0;
        }

        public int GetNodeCount()
        {
            return nodes.Count;
        }

    }
}

