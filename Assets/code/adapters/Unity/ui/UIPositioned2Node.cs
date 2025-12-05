using Domain;
using System;
using UnityEngine;


namespace GraphMaster.UnityAdapter.UI
{
    public class UIPositioned2Node : MonoBehaviour, Domain.GraphNodeInterface
    {
        [SerializeField] private NodeVisualEffects visualEffects;
        [SerializeField] private GraphMaster.UnityAdapter.Positioned2Node sourse;

        public event Action<string> IsSelected;
        public event Action<string> IsDeselected;
        private bool isSelected = false;
        
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


        public void Initialize(string name, Vector3 position, int squenseCount)
        {
            visualEffects.Initialize(squenseCount);
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
            visualEffects.StartSelectionAnimation();
            IsSelected?.Invoke(this.GetName());
        }

        public void DeselectThisNode()
        {
            if (isSelected)
            {
                visualEffects.StartDeselectionAnimation();
                isSelected = false;
                IsDeselected?.Invoke(this.GetName());
            }
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
            visualEffects.UpdateNameDisplay(name);
            sourse.SetName(name);
        }

        public void SetDescription(string description)
        {
            sourse.SetDescription(description);
        }
    }
}


