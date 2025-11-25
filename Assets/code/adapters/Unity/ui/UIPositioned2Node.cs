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


        GraphMaster.UnityAdapter.Positioned2Node sourse;
        public event Action IsSelected; 
        private bool isSelected = false;
        private bool drag = false;
        private Vector2 startDragPosition;
    
        void Start()
        {
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
                isSelected = true;
                selectAnimation();
                IsSelected?.Invoke();
            }
            else
            {
                deselectionAnimation();
                isSelected = false;
            }
        
        }

        private void OnMouseDrag()
        {
            if (drag)
            {
                Vector2 deltaDrag = startDragPosition - new Vector2(Input.mousePosition.x, Input.mousePosition.y);
                Debug.Log(sourse == null);
                sourse.SetPosition(startDragPosition - deltaDrag);
            }
        }


        private void OnMouseUp()
        {
            drag = false;
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


