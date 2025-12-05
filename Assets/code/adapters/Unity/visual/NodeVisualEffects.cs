using UnityEngine;
using TMPro;

namespace GraphMaster.UnityAdapter.VisualEffects
{
    public class NodeVisualEffects : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI nameVisual;
        [SerializeField] private Canvas nodeToolBar;
        [SerializeField] private Color selectedColor;
        [SerializeField] private Color defaultColor;
        [SerializeField] private Vector2 defaultScale;
        [SerializeField] private Vector2 selectedScale;
        [SerializeField] private SpriteRenderer nodeSpriteRenderer;

        public void Initialize(int squenseCount)
        {
            CheckGameObjectContent();
            SetVisualLayer(squenseCount);
        }

        public void CheckGameObjectContent()
        {
            if (nameVisual == null)
            {
                throw new System.Exception("TextMeshProUGUI nameVisual can't be a null");
            }

            if (nodeToolBar == null)
            {
                throw new System.Exception("Canvas nodeToolBar can't be a null");
            }

            if (nodeSpriteRenderer == null)
            {
                throw new System.Exception("SpriteRenderer nodeSpriteRenderer can't be a null");
            }
        }

        public void StartSelectionAnimation()
        {
            nodeSpriteRenderer.color = selectedColor;
            transform.localScale = selectedScale;
        }

        public void StartDeselectionAnimation()
        {
            nodeSpriteRenderer.color = defaultColor;
            transform.localScale = defaultScale;
        }

        public void UpdateNameDisplay(string name)
        {
            if (nameVisual != null)
            {
                nameVisual.text = name;
            }
        }

        private void SetVisualLayer(int graphEdgesSequenseCount)
        {
            nodeToolBar.sortingOrder = graphEdgesSequenseCount * 2 + 1;
            nodeSpriteRenderer.sortingOrder = graphEdgesSequenseCount * 2;
        }

        public void UpdateFrame()
        {
            return;
        }
    }
}

