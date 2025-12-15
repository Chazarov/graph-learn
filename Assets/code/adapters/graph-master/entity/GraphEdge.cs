using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Domain;
using UnityEditor.Experimental.GraphView;

namespace GraphMaster
{
    public class GraphEdge<TNode>: Domain.GraphEdgeInterface<TNode> where TNode : GraphNodeInterface , GraphPartInterface
    {
        private float weight;
        private bool hasWeight = false;
        private string name;

        private TNode targetNode;
        private TNode sourceNode;

        


        // Конструктор для взвешенного ребра
        // 1: 
        public GraphEdge(TNode sourceNode, TNode targetNode, float weight)
        {
            this.targetNode = targetNode;
            this.sourceNode = sourceNode;
            SetWeight(weight);
        }
        // Конструктор для невзвешенного ребра
        public GraphEdge(TNode sourceNode, TNode targetNode): this(sourceNode, targetNode, 1){}
   
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

        public TNode GetSourceNode()
        {
            return this.sourceNode;
        }

        public TNode GetTargetNode()
        {
            return this.targetNode;
        }

        public string GetName()
        {
            return name;
        }

        public void SetName(string name)
        {
            this.name = name;
        }

        public void SetSourseNode(TNode node)
        {
            this.sourceNode = node;
        }

        public void SetTargetNode(TNode node)
        {
            this.targetNode= node;
        }
    }

}
