using Domain;
using GraphMaster.UnityAdapter.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GraphMaster.UnityAdapter
{
    public class EdgeVisual : MonoBehaviour, Domain.GraphEdgeInterface<UIPositioned2Node>
    {
        [SerializeField] LineRenderer line;

        GraphEdgeInterface<UIPositioned2Node> sourse;
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
            else
            {
                Debug.Log("Boo");
            }
        }

        public void Initialize(UIPositioned2Node sourseNode, UIPositioned2Node targetNode, string edgeName)
        {
            GraphEdgeInterface<UIPositioned2Node> edge = new GraphEdge<UIPositioned2Node>(sourseNode, targetNode);
            
            this.sourse = edge;
            this.name = $"Edge {edgeName}";
            this.sourse.SetName(edgeName);

            this.SetSourseNode(sourseNode);
            this.SetTargetNode(targetNode);

            line.positionCount = 2;
            line.SetPosition(0, sourseNode.transform.position);
            line.SetPosition(1, targetNode.transform.position);
        }

        public UIPositioned2Node GetSourceNode()
        {
            return sourceNode;
        }

        public UIPositioned2Node GetTargetNode()
        {
            return targetNode;
        }

        public float GetWeight()
        {
            return sourse.GetWeight();
        }

        public bool HasWeight()
        {
            return sourse.HasWeight();
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

        public void SetTargetNode(UIPositioned2Node node)
        {
            this.targetNode = node;
            sourse.SetTargetNode(node);
        }
    }

}

