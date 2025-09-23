using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace GraphMaster
{
    public class GraphEdge : domain.GraphEdgeInterface
    {
        private float weight;
        private bool hasWeight = false;

        private Graph graph;
        private GraphNode targetNode;
        private GraphNode sourceNode;

        public GraphEdge(GraphNode sourceNode, GraphNode targetNode, Graph graph)
        {
            if(sourceNode == null || targetNode == null)
            {
                throw new ArgumentNullException("Source and target nodes cannot be null");
            }

            this.sourceNode = sourceNode;
            this.targetNode = targetNode;
            this.graph = graph;

            // Проверка на петли
            if (sourceNode == targetNode && !graph.AllowedLoops())
            {
                throw new LoopNotAllowed();
            }

            sourceNode.AddEdge(this);
        }

        public GraphEdge(GraphNode sourceNode, GraphNode targetNode, float weight) : this(sourceNode, targetNode)
        {
            SetWeight(weight);
        }
   
        public float GetWeight()
        {
            if (!graph.IsWeighed())
            {
                throw new WeightNotAllowedException("Cannot get weight from edge in unweighted graph");
            }
            if (!hasWeight)
            {
                throw new WeightRequiredException("Edge weight has not been set");
            }
            return this.weight;
        }

        public void SetWeight(float weight)
        {
            if (!graph.IsWeighed())
            {
                throw new WeightNotAllowedException("Cannot set weight on edge in unweighted graph");
            }
            if (weight < 0 && !graph.AllowedNegativeEdges())
            {
                throw new NegativeEdgeNotAllowed($"Negative weight {weight} is not allowed in this graph");
            }
            this.weight = weight;
            this.hasWeight = true;
        }

        public bool HasWeight()
        {
            return hasWeight;
        }

        public GraphNode GetSourceNode()
        {
            return this.sourceNode;
        }

        public GraphNode GetTargetNode()
        {
            return this.targetNode;
        }

        public bool IsParralel(GraphEdge other)
        {
            if (other == null) return false;
            // Параллельные ребра - это ребра, которые соединяют одни и те же вершины
            return other.GetTargetNode() == this.GetTargetNode() && 
                   other.GetSourceNode() == this.GetSourceNode();
        }
    }

}
