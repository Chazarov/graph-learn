using Domain;
using GraphMaster.UnityAdapter.UI;
using GraphMaster.UnityAdapter.VisualEffects;
using System;
using UnityEngine;

namespace GraphMaster.UnityAdapter
{
    public class EdgeUI : MonoBehaviour, Domain.GraphEdgeInterface<NodeUI>, GraphObjectUiActionsInterface
    {
        [SerializeField] private EdgeVisualEffects visualEffects;

        private GraphEdgeInterface<NodeUI> sourse;
        private NodeUI sourceNode;
        private NodeUI targetNode;

        public bool isSelected = false;

        public event Action<EdgeUI> IsSelected;
        public event Action<EdgeUI> IsDeselected;

        void Update()
        {
            visualEffects?.UpdateFrame();
        }

        private void OnMouseDown()
        {
            if (!isSelected)
            {
                Select();
                isSelected = true;
            }
            else
            {
                isSelected = false;
                Deselect();
            }
        }

        public void Select()
        {
            visualEffects?.SelectThisAnimation();
            IsSelected?.Invoke(this);
        }

        public void Deselect()
        {
            visualEffects?.DeselectThisAnimation();
            IsDeselected?.Invoke(this);
        }

        public void Initialize(NodeUI sourseNode, NodeUI targetNode, string edgeName, int graphEdgesSequenseCount)
        {
            CheckGameObjectContent();
            
            GraphEdgeInterface<NodeUI> edge = new GraphEdge<NodeUI>(sourseNode, targetNode);
            
            this.sourse = edge;
            this.name = $"Edge {edgeName}";
            this.sourse.SetName(edgeName);

            this.SetSourseNode(sourseNode);
            this.SetTargetNode(targetNode);
            
            visualEffects?.Initialize(graphEdgesSequenseCount, sourseNode, targetNode);
            
            this.SetWeight(sourse.GetWeight());
        }

        public void CheckGameObjectContent()
        {
            if (visualEffects == null)            {
                Debug.LogWarning("EdgeVisualEffects component is not assigned. Visual features will be disabled.");
            }
            else
            {
                visualEffects?.CheckGameObjectContent();
            }
        }

        public NodeUI GetSourceNode()
        {
            return sourceNode;
        }

        public NodeUI GetTargetNode()
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

        public void SetSourseNode(NodeUI node)
        {
            this.sourceNode = node;
            sourse.SetSourseNode(node);
        }

        public void SetTargetNode(NodeUI node)
        {
            this.targetNode = node;
            sourse.SetTargetNode(node);
        }

        public void PointThis()
        {
            visualEffects?.PointThisAnimation();
        }

        public void RemovePointer()
        {
            visualEffects?.RemovePointerAnimation();
        }

        public void MarkThis()
        {
            visualEffects?.MarkThisAnimation();
        }

        public void RemoveMark()
        {
            visualEffects?.RemoveMarkAnimation();
        }
        
    }

}

