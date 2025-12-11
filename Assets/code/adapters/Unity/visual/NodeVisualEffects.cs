using UnityEngine;
using TMPro;

namespace GraphMaster.UnityAdapter.VisualEffects
{
    public class NodeVisualEffects : MonoBehaviour, GraphObjectVisualEffectsInterface
    {
        [SerializeField] private TextMeshProUGUI nameVisual;
        [SerializeField] private Canvas nodeToolBar;

        [Header("Selection Animation")]
        [SerializeField] private Color selectedColor;
        [SerializeField] private Vector2 selectedScale;
        private Color defaultColor;
        private Vector2 defaultScale;

        [SerializeField] private SpriteRenderer nodeSpriteRenderer;
        
        [Header("Mark Animation")]
        [SerializeField] private SpriteRenderer markSpriteRenderer;
        [SerializeField] private Animator markAnimator;
        

        [Header("Root Animation")]
        [SerializeField] private SpriteRenderer rootSpriteRenderer;


        private void Start()
        {
            defaultColor = nodeSpriteRenderer.color;
            defaultScale = transform.localScale;
        }

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

            if (markSpriteRenderer == null)
            {
                throw new System.Exception("SpriteRenderer markSpriteRenderer can't be a null");
            }
        }

        public void MarkAsRootAnimation()
        {
            this.rootSpriteRenderer.gameObject.SetActive(true);
        }

        public void RemoveRootMarkAnimation()
        {
            this.rootSpriteRenderer.gameObject.SetActive(false);
        }

        public void SelectThisAnimation()
        {

            nodeSpriteRenderer.color = selectedColor;
            transform.localScale = selectedScale;

        }

        public void DeselectThisAnimation()
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
            int baseVisualLayer = graphEdgesSequenseCount * 3;
            nodeToolBar.sortingOrder = baseVisualLayer + 1;
            markSpriteRenderer.sortingOrder = baseVisualLayer + 2;
            nodeSpriteRenderer.sortingOrder = baseVisualLayer;
        }

        public void UpdateFrame()
        {
            return;
        }


        public void MarkThisAnimation()
        {
            if (markAnimator != null)
            {
                markAnimator.SetBool("Mark", true);
            }
        }

        public void RemoveMarkAnimation()
        {
            if (markAnimator != null)
            {
                markAnimator.SetBool("Mark", false);
            }
        }

    }
}

