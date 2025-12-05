using Domain;
using GraphMaster.UnityAdapter.VisualEffects;
using System;
using UnityEngine;


namespace GraphMaster.UnityAdapter.UI
{
    public class NodeUI : MonoBehaviour, Domain.GraphNodeInterface, GraphObjectUiActionsInterface
    {
        [SerializeField] private NodeVisualEffects visualEffects;
        [SerializeField] private GraphMaster.UnityAdapter.Positioned2Node sourse;

        public event Action<string> IsSelected;
        public event Action<string> IsDeselected;
        private bool isSelected = false;
        
        private void OnMouseDown()
        {
            this.Select();
        }

        private void OnMouseDrag()
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = transform.position.z;
            sourse.SetPosition(mousePosition);
        }


        public void Initialize(string name, Vector3 position, int squenseCount)
        {
            CheckGameobgectContent();
            
            visualEffects?.Initialize(squenseCount);
            SetName(name);
            SetPosition(position);
        }

        public void CheckGameobgectContent()
        {
            if (sourse == null)
            {
                throw new System.Exception("sourse can't be a null. Please add a sourse Positioned2NodeComponent ");
            }

            if (visualEffects == null)
            {
                Debug.LogWarning("NodeVisualEffects component is not assigned. Visual features will be disabled.");
            }
            else
            {
                visualEffects.CheckGameObjectContent();
            }
        }

        public void SetPosition(Vector3 position)
        {
            sourse.SetPosition(position);
        }

        public void Select()
        {
            isSelected = true;
            visualEffects?.StartSelectionAnimation();
            IsSelected?.Invoke(this.GetName());
        }

        public void Deselect()
        {
            if (isSelected)
            {
                visualEffects?.StartDeselectionAnimation();
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
            visualEffects?.UpdateNameDisplay(name);
            sourse.SetName(name);
        }

        public void SetDescription(string description)
        {
            sourse.SetDescription(description);
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


