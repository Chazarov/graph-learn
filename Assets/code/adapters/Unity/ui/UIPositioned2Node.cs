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
    
        void Start()
        {

        }
        
        void Update()
        {
        
        }
        
        private void OnMouseDown()
        {
            this.SelectThisNode();
        }

        private void OnMouseDrag()
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = transform.position.z;
            sourse.SetPosition(mousePosition);
        }


        public void Initialize(string name, Vector3 position)
        {
            SetName(name);
            SetPosition(position);
        }

        public void CheckGameobgectContent()
        {
            if (sourse == null)
            {
                throw new System.Exception("sourse can't be a null. Please add a sourse Positioned2NodeComponent ");
            }
        }

        public void SetPosition(Vector3 position)
        {
            sourse.SetPosition(position);
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


