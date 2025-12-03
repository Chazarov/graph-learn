using Domain;
using GraphMaster.UnityAdapter.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace GraphMaster.UnityAdapter
{
    public class EdgeVisual : MonoBehaviour, Domain.GraphEdgeInterface<UIPositioned2Node>
    {
        [SerializeField] private LineRenderer line;
        
        [SerializeField] private LineRenderer selectedLine;
        [SerializeField] private EdgeCollider2D edgeCollider;
        [SerializeField] private TextMeshProUGUI weightText;
        [SerializeField] private Canvas edgeToolBar;

        private LineRenderer activeLine;
        private GraphEdgeInterface<UIPositioned2Node> sourse;
        private UIPositioned2Node sourceNode;
        private UIPositioned2Node targetNode;

        public bool isSelected = false;

        public event Action<EdgeVisual> IsSelected;
        public event Action<EdgeVisual> IsDeselected;

        void Start()
        {
            activeLine = line;
            selectedLine.gameObject.SetActive(false);
        }


        void Update()
        {
            if (sourceNode != null && targetNode != null)
            {
                activeLine.SetPosition(0, sourceNode.transform.position);
                activeLine.SetPosition(1, targetNode.transform.position);
                edgeCollider.SetPoints(new List<Vector2> { sourceNode.transform.position, targetNode.transform.position });
                
                UpdateWeightTextPosition();
            }
        }

        private void UpdateWeightTextPosition()
        {
            if (edgeToolBar == null || sourceNode == null || targetNode == null) return;

            Vector3 sourcePos = sourceNode.transform.position;
            Vector3 targetPos = targetNode.transform.position;
            Vector3 centerPos = (sourcePos + targetPos) / 2f;

            edgeToolBar.transform.position = centerPos;

            Vector3 direction = (targetPos - sourcePos).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            if (angle > 90f || angle < -90f)
            {
                angle += 180f;
            }

            edgeToolBar.transform.rotation = Quaternion.Euler(0, 0, angle);
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
            line.gameObject.SetActive(false);
            selectedLine.gameObject.SetActive(true);
            activeLine = selectedLine;
            IsSelected?.Invoke(this);
        }

        public void DeselectThisEdge()
        {
            selectedLine.gameObject.SetActive(false);
            line.gameObject.SetActive(true);
            activeLine = line;
            IsDeselected?.Invoke(this);
        }

        public void Initialize(UIPositioned2Node sourseNode, UIPositioned2Node targetNode, string edgeName)
        {
            CheckGameObjectContent();
            activeLine = line;
            GraphEdgeInterface<UIPositioned2Node> edge = new GraphEdge<UIPositioned2Node>(sourseNode, targetNode);
            
            this.sourse = edge;
            this.name = $"Edge {edgeName}";
            this.sourse.SetName(edgeName);

            this.SetSourseNode(sourseNode);
            this.SetTargetNode(targetNode);

            activeLine.positionCount = 2;
            activeLine.SetPosition(0, sourseNode.transform.position);
            activeLine.SetPosition(1, targetNode.transform.position);
            
            selectedLine.positionCount = 2;


            edgeCollider.SetPoints(new List<Vector2> { sourseNode.transform.position , targetNode.transform.position });
        }


        public void CheckGameObjectContent()
        {
            if (line == null)
            {
                throw new System.Exception(" Line Renderer can't be a null");
            }


            if (selectedLine == null)
            {
                throw new System.Exception(" SelectedLine Renderer can't be a null");
            }

            if (edgeCollider == null)
            {
                throw new System.Exception(" EdgeCollider2D  edgeCollider can't be a null");
            }

            if (weightText == null)
            {
                throw new System.Exception(" TextMeshPro weightText can't be a null");
            }

            if (edgeToolBar == null)
            {
                throw new System.Exception(" Canvas edgeToolBar can't be a null");
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
            if (weightText != null)
            {
                weightText.text = weight.ToString();
            }
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

