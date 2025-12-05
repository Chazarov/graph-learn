using Domain;
using GraphMaster.UnityAdapter.UI;
using System;
using UnityEngine;

namespace GraphMaster.UnityAdapter
{
    public class EdgeVisual : MonoBehaviour, Domain.GraphEdgeInterface<UIPositioned2Node>
    {
        [SerializeField] private EdgeVisualEffects visualEffects;

        private GraphEdgeInterface<UIPositioned2Node> sourse;
        private UIPositioned2Node sourceNode;
        private UIPositioned2Node targetNode;

        public bool isSelected = false;

        public event Action<EdgeVisual> IsSelected;
        public event Action<EdgeVisual> IsDeselected;

        void Update()
        {
            visualEffects?.UpdateFrame(transform.position.z);
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

        public void SelectThisEdge()
        {
            visualEffects?.StartSelectionAnimation();
            IsSelected?.Invoke(this);
        }

        public void DeselectThisEdge()
        {
            visualEffects?.StartDeselectionAnimation();
            IsDeselected?.Invoke(this);
        }

        public void Initialize(UIPositioned2Node sourseNode, UIPositioned2Node targetNode, string edgeName, int graphEdgesSequenseCount)
        {
            GraphEdgeInterface<UIPositioned2Node> edge = new GraphEdge<UIPositioned2Node>(sourseNode, targetNode);
            
            this.sourse = edge;
            this.name = $"Edge {edgeName}";
            this.sourse.SetName(edgeName);

            this.SetSourseNode(sourseNode);
            this.SetTargetNode(targetNode);
            
            visualEffects?.Initialize(graphEdgesSequenseCount, sourseNode, targetNode);
            
            this.SetWeight(sourse.GetWeight());
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
            visualEffects?.UpdateWeightDisplay(weight);
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

