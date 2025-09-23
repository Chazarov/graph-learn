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

        private Graph graph;
        private GraphNode targetNode;
        private GraphNode sourceNode;

        private void init(GraphNode sourceNode, GraphNode targetNode)
        {
            if (sourceNode == null || targetNode == null)
                throw new InvalidGraphOperationException("Source and target nodes cannot be null");
            if (sourceNode.GetGraph() != targetNode.GetGraph())
                throw new InvalidGraphOperationException("Vertices of the same edge cannot belong to different graphs");
            if (sourceNode == targetNode && !graph.AllowedLoops())
                throw new LoopNotAllowed();

            this.sourceNode = sourceNode;
            this.targetNode = targetNode;
            this.graph = targetNode.GetGraph();

            sourceNode.AddEdge(this);
            targetNode.AddEdge(this);
        }

        private void ValidateWeightUsage(bool isWeighted)
        {
            if (graph == null)
                throw new InvalidGraphOperationException("Graph can't be null");

            if (isWeighted && !graph.IsWeighed())
                throw new WeightNotAllowedException("Edge cannot have weight in an unweighted graph");

            if (!isWeighted && graph.IsWeighed())
                throw new WeightRequiredException("Edges for a weighted graph must be created with an indication of the weight.");
        }

        public GraphEdge(GraphNode sourceNode, GraphNode targetNode)
        {
            init(sourceNode, targetNode);
            ValidateWeightUsage(false);
            
        }

        public GraphEdge(GraphNode sourceNode, GraphNode targetNode, float weight)
        {
            init(sourceNode, targetNode);
            ValidateWeightUsage(true);
            SetWeight(weight);
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
