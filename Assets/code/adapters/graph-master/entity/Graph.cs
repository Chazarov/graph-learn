using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Domain;
using System;
using System.Linq;
using Unity.VisualScripting;
using System.Xml.Linq;
using Unity.Collections;

namespace GraphMaster
{
    public class MyGraph<TNode, TEdge> : GraphInterface<TNode, TEdge> where TNode : Domain.GraphNodeInterface where TEdge : Domain.GraphEdgeInterface
    {
        private bool isOriented = false;
        private bool isWeighed = false;
        private bool allowParallelEdges = false;
        private bool allowNegativeEdges = false;
        private bool allowLoops = false;

        private List<TNode> nodes = new List<TNode>();
        private List<TEdge> edges = new List<TEdge>();
        private Dictionary<string, TNode> nodesMap = new Dictionary<string, TNode>();




        public TNode GetNode(string name)
        {
            var node = nodesMap[name];
            if (node == null)
            {
                throw new NodeNotFoundException(name);
            }
            return node;
        }

        public List<TNode> GetNodes()
        {
            return new List<TNode>(this.nodes);
        }

        public List<TEdge> GetEdges()
        {
            return new List<TEdge>(this.edges);
        }


        public TEdge AddEdge(TEdge edge)
        {
            if (edges.Contains(edge))
            {
                throw new DublicateException("It is not possible to add the same edge twice.");
            }
            edges.Add(edge);
            return edge;
        }

        public TNode AddNode(TNode node)
        {
            if (nodes.Contains(node))
            {
                throw new DublicateException("It is not possible to add the same node twice.");
            }
            if (nodesMap[node.GetName()] != null)
            {
                throw new DublicateException("It is not possible to add the node with same name twice.");
            }
            nodesMap.Add(node.GetName(), node);
            nodes.Add(node);
            return node;
        }


        public void DeleteNode(string name)
        {
            var node = nodesMap[name];
            if (node == null) {
                throw new NodeNotFoundException(name);
            }
            List<GraphEdgeInterface> nodeEdges = node.GetEdges();
            foreach (var edge in nodeEdges)
            {
                // Возможно оптимизировать
                edge.GetSourceNode().DisconnectEdge(edge);
                edge.GetTargetNode().DisconnectEdge(edge);
            }
            this.nodes.Remove(node);
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

