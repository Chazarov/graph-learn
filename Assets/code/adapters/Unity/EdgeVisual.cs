using Domain;
using GraphMaster.UnityAdapter.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GraphMaster.UnityAdapter
{
    public class EdgeVisual : MonoBehaviour, Domain.GraphEdgeInterface<UIPositioned2Node>
    {
        [SerializeField] LineRenderer line;
        [SerializeField] EdgeCollider2D edgeCollider;

        GraphEdgeInterface<UIPositioned2Node> sourse;
        private UIPositioned2Node sourceNode;
        private UIPositioned2Node targetNode;

        public bool isSelected = false;

        public event Action<EdgeVisual> IsSelected;
        public event Action<EdgeVisual> IsDeselected;

        void Start()
        {

        }


        void Update()
        {
            if (sourceNode != null && targetNode != null)
            {
                line.SetPosition(0, sourceNode.transform.position);
                line.SetPosition(1, targetNode.transform.position);
                edgeCollider.SetPoints(new List<Vector2> { sourceNode.transform.position, targetNode.transform.position });
            }
        }

        private void OnMouseDown()
        {
            if (!isSelected)
            {
                SelectThisEdge();
                isSelected = true;
            }
            else
            {
                isSelected = false;
                DeselectThisEdge();
            }
        }

        private void SelectThisEdge()
        {
            line.startColor = Color.yellow;
            line.endColor = Color.yellow;
            line.startWidth = 0.15f;
            line.endWidth = 0.15f;
            IsSelected?.Invoke(this);
        }

        private void DeselectThisEdge()
        {
            line.startColor = Color.white;
            line.endColor = Color.white;
            line.startWidth = 0.1f;
            line.endWidth = 0.1f;
            IsDeselected?.Invoke(this);
        }

        public void Initialize(UIPositioned2Node sourseNode, UIPositioned2Node targetNode, string edgeName)
        {
            CheckGameObjectContent();
            GraphEdgeInterface<UIPositioned2Node> edge = new GraphEdge<UIPositioned2Node>(sourseNode, targetNode);
            
            this.sourse = edge;
            this.name = $"Edge {edgeName}";
            this.sourse.SetName(edgeName);

            this.SetSourseNode(sourseNode);
            this.SetTargetNode(targetNode);

            line.positionCount = 2;
            line.SetPosition(0, sourseNode.transform.position);
            line.SetPosition(1, targetNode.transform.position);


            edgeCollider.SetPoints(new List<Vector2> { sourseNode.transform.position , targetNode.transform.position });
        }


        public void CheckGameObjectContent()
        {
            if (line == null)
            {
                throw new System.Exception(" Line Renderer can't be a null");
            }

            if (edgeCollider == null)
            {
                throw new System.Exception(" EdgeCollider2D  edgeCollider can't be a null");
            }
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

