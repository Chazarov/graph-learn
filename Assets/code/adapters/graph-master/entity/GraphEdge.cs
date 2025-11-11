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

        private GraphNodeInterface targetNode;
        private GraphNodeInterface sourceNode;

        // Конструктор для невзвешенного ребра
        public GraphEdge(GraphNodeInterface sourceNode, GraphNodeInterface targetNode)
        {
            this.targetNode = targetNode;
            this.sourceNode = sourceNode;
        }


        // Конструктор для взвешенного ребра
        // 1: 
        public GraphEdge(GraphNodeInterface sourceNode, GraphNodeInterface targetNode, float weight)
        {
            this.targetNode = targetNode;
            this.sourceNode = sourceNode;
            SetWeight(weight);
        }

   
        public float GetWeight()
        {
            return this.weight;
        }

        public void SetWeight(float weight)
        {
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
