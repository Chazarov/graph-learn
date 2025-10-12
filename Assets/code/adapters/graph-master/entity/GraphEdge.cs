using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Domain;

namespace GraphMaster
{
    public class GraphEdge : GraphEdgeInterface
    {
        private float weight;
        private bool hasWeight = false;

        private GraphInterface<GraphNodeInterface> graph;
        private GraphNodeInterface targetNode;
        private GraphNodeInterface sourceNode;

        // Конструктор для невзвешенного ребра
        public GraphEdge(GraphNodeInterface sourceNode, GraphNodeInterface targetNode)
        {
            InitializeEdge(sourceNode, targetNode);
            
            // Проверка совместимости с типом графа
            if (graph.IsWeighed())
                throw new WeightRequiredException("Edges for a weighted graph must be created with an indication of the weight.");
        }

        // Конструктор для взвешенного ребра
        public GraphEdge(GraphNodeInterface sourceNode, GraphNodeInterface targetNode, float weight)
        {
            InitializeEdge(sourceNode, targetNode);

            // Проверка совместимости с типом графа
            if (!graph.IsWeighed())
                throw new WeightNotAllowedException("Edge cannot have weight in an unweighted graph");
            
            SetWeight(weight);
        }

        private void InitializeEdge(GraphNodeInterface sourceNode, GraphNodeInterface targetNode)
        {
            // Валидация параметров
            if (sourceNode == null || targetNode == null)
                throw new InvalidGraphOperationException("Source and target nodes cannot be null");
            if (sourceNode.GetGraph() != targetNode.GetGraph())
                throw new InvalidGraphOperationException("Vertices of the same edge cannot belong to different graphs");

            this.sourceNode = sourceNode;
            this.targetNode = targetNode;
            this.graph = targetNode.GetGraph();

            if (sourceNode == targetNode && !graph.AllowedLoops())
                throw new LoopNotAllowed();

            sourceNode.AddEdge(this);
            targetNode.AddEdge(this);
        }
        public void DeleteEdge()
        {
            this.GetSourceNode().DisconnectEdge(this);
            this.GetTargetNode().DisconnectEdge(this);
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

        public GraphNodeInterface GetSourceNode()
        {
            return this.sourceNode;
        }

        public GraphNodeInterface GetTargetNode()
        {
            return this.targetNode;
        }

        public bool IsParralel(GraphEdgeInterface other)
        {
            if (other == null) return false;
            return other.GetTargetNode() == this.GetTargetNode() && 
                   other.GetSourceNode() == this.GetSourceNode();
        }
    }

}
