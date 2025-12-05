using UnityEngine;
using TMPro;

namespace GraphMaster.UnityAdapter.UI
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
            SetVisualLayer(squenseCount);
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
    }
}

