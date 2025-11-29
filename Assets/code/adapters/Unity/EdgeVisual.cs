using Domain;
using GraphMaster.UnityAdapter.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GraphMaster.UnityAdapter
{
    public class EdgeVisual : MonoBehaviour, Domain.GraphEdgeInterface
    {
        [SerializeField] LineRenderer line;

        GraphEdgeInterface sourse;
        private UIPositioned2Node sourceNode;
        private UIPositioned2Node targetNode;

        void Start()
        {

        }

        void Update()
        {
            if (sourceNode != null && targetNode != null)
            {
                line.SetPosition(0, sourceNode.transform.position);
                line.SetPosition(1, targetNode.transform.position);
            }
        }

        public void Initialize(UIPositioned2Node sourseNode, UIPositioned2Node targetNode, string edgeName)
        {
            GraphEdgeInterface edge = new GraphEdge(sourseNode, targetNode);
            
            this.sourse = edge;
            this.sourse.SetName(edgeName);

            line.positionCount = 2;
            line.SetPosition(0, sourseNode.transform.position);
            line.SetPosition(1, targetNode.transform.position);
        }

        public GraphNodeInterface GetSourceNode()
        {
            return sourse.GetSourceNode();
        }

        public GraphNodeInterface GetTargetNode()
        {
            return sourse.GetTargetNode();
        }

        public float GetWeight()
        {
            return sourse.GetWeight();
        }

        public bool HasWeight()
        {
            return sourse.HasWeight();
        }

        public bool IsParralel(GraphEdgeInterface other)
        {
            return sourse.IsParralel(other);
        }

        public void SetWeight(float weight)
        {
            sourse.SetWeight(weight);
        }

        public string GetName()
        {
            return sourse.GetName();
        }

        public void SetName(string name)
        {
            sourse.SetName(name);
        }

        public void SetSourseNode(UIPositioned2Node node)
        {
            this.sourceNode = node;
            sourse.SetSourseNode(node);
        }

        public void SetTargetNode(GraphNodeInterface node)
        {
            this.targetNode = node;
            sourse.SetTargetNode(node);
        }
    }

}

