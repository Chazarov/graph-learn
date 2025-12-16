using Domain;
using GraphMaster.UnityAdapter.VisualEffects;
using System;
using UnityEngine;


namespace GraphMaster.UnityAdapter.UI
{
    public class NodeUI : MonoBehaviour, Domain.GraphNodeInterface, GraphPartInterface, GraphObjectUiActionsInterface
    {
        [SerializeField] public NodeVisualEffects VisualEffects;


        [SerializeField] private GraphMaster.UnityAdapter.Positioned2Node sourse;

        [SerializeField] private UiActionsManager uiActionsManager;

        public event Action<string> IsSelected;
        public event Action<string> IsDeselected;
        public event Action<NodeUI> IsRootMarking;
        public event Action<NodeUI> IsRootUnmarking;

        

        private bool isSelected = false;

        private bool isRoot = false;

        [SerializeField][Range(0f, 1f)] private float maxDelayOfDoublePressing;

        private float prevPressTime = 0;
        
        private void OnMouseDown()
        {
            if((Time.time - prevPressTime) < maxDelayOfDoublePressing)
            {
                MarkAsRoot();
            }
            this.Select();
            prevPressTime = Time.time;
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

            
            
            VisualEffects?.Initialize(squenseCount);
            SetName(name);
            SetPosition(position);
        }

        public void MarkAsRoot()
        {
            if (!uiActionsManager.GetRootReplacementIsAllowed()) return;
            if (isRoot) return;
            IsRootMarking.Invoke(this);
            MarkAsRootWithoutNotify(); 
        }

        public void MarkAsRootWithoutNotify()
        {
            if (!uiActionsManager.GetRootReplacementIsAllowed()) return;
            if (isRoot) return;
            VisualEffects?.MarkAsRootAnimation();
            isRoot = true;
        }

        public void RemoveRoot()
        {
            VisualEffects.RemoveRootMarkAnimation();
            IsRootUnmarking?.Invoke(this);
            isRoot = false;

        }

        public void CheckGameobgectContent()
        {
            if (sourse == null)
            {
                throw new System.Exception("sourse can't be a null. Please add a sourse Positioned2NodeComponent ");
            }

            if (VisualEffects == null)
            {
                Debug.LogWarning("NodeVisualEffects component is not assigned. Visual features will be disabled.");
            }
            else
            {
                VisualEffects.CheckGameObjectContent();
            }
        }

        public void SetPosition(Vector3 position)
        {
            sourse.SetPosition(position);
        }

        public void Select()
        {
            isSelected = true;
            VisualEffects?.SelectThisAnimation();
            IsSelected?.Invoke(this.GetName());
        }

        public void Deselect()
        {
            if (isSelected)
            {
                VisualEffects?.DeselectThisAnimation();
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
            VisualEffects?.UpdateNameDisplay(name);
            sourse.SetName(name);
        }

        public void SetDescription(string description)
        {
            sourse.SetDescription(description);
        }


        public Vector3 GetCenterPosition()
        {
            return transform.position;
        }

        public GraphObjectVisualEffectsInterface GetVisualEffects()
        {
            return VisualEffects;
        }

    }
}


