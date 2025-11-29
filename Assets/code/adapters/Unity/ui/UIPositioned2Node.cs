using Domain;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


namespace GraphMaster.UnityAdapter.UI
{
    public class UIPositioned2Node : MonoBehaviour, Domain.GraphNodeInterface
    {
        [SerializeField] private TextMeshProUGUI nameVisual;
        [SerializeField] private Color selectedColor;
        [SerializeField] private Color defaultColor;
        [SerializeField] private Vector2 defaultScale;
        [SerializeField] private Vector2 selectedScale;
        [SerializeField] private SpriteRenderer nodeSpriteRenderer;
        [SerializeField] private GraphMaster.UnityAdapter.Positioned2Node sourse;
        

        public event Action<string> IsSelected;
        public event Action<string> IsDeselected;
        private bool isSelected = false;
        private bool drag = false;
        private Vector2 startDragPosition;
    
        void Start()
        {

        }

        public void CheckGameobgectContent()
        {
            if (sourse == null)
            {
                throw new System.Exception("sourse can't be a null. Please add a sourse Positioned2NodeComponent ");
            }
        }

        void Update()
        {
        
        }


        private void OnMouseDown()
        {
            drag = true;
            startDragPosition = Input.mousePosition;

            if (!isSelected)
            {
                this.SelectThisNode();
            }
            else
            {
                this.DeselectThisNode();
            }
        
        }

        private void OnMouseDrag()
        {
            //if (drag)
            //{
            //    Vector2 deltaDrag = startDragPosition - new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            //    sourse.SetPosition(startDragPosition - deltaDrag);
            //}
        }


        private void OnMouseUp()
        {
            drag = false;
        }

        public void SelectThisNode()
        {
            isSelected = true;
            selectAnimation();
            IsSelected?.Invoke(this.GetName());
        }

        public void DeselectThisNode()
        {
            if (isSelected)
            {
                deselectionAnimation();
                isSelected = false;
                IsDeselected?.Invoke(this.GetName());
            }
            
        }

        private void selectAnimation()
        {
            nodeSpriteRenderer.color = selectedColor;
            transform.localScale = selectedScale;
        }

        private void deselectionAnimation()
        {
            nodeSpriteRenderer.color = defaultColor;
            transform.localScale = defaultScale;
        }




        public string GetName()
        {
            return sourse.GetName();
        }

        public string GetDescription()
        {
            return sourse.GetDescription();
        }

        public void DisconnectEdge(GraphEdgeInterface edge)
        {
            sourse.DisconnectEdge(edge);
        }

        public List<GraphEdgeInterface> GetEdges()
        {
            return sourse.GetEdges();
        }

        public void AddEdge(GraphEdgeInterface edge)
        {
            sourse.AddEdge(edge);
        }

        public void SetName(string name)
        {
            this.nameVisual.text = name;
            sourse.SetName(name);
        }

        public void SetDescription(string description)
        {
            sourse.SetDescription(description);
        }
    }
}


