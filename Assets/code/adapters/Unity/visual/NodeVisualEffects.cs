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
        [SerializeField] private string markApplyAnimationName;
        [SerializeField] private string markRevertAnimationName;
        [SerializeField] private Animator nodeAnimator;
        
        [Header("Point Animation")]
        [SerializeField] private Transform pointerObject;
        [SerializeField] private float pointerAnimationDuration = 0.5f;
        
        private Coroutine currentPointerCoroutine;

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

        public void PointThisAnimation()
        {
            if (pointerObject != null)
            {
                if (currentPointerCoroutine != null)
                {
                    StopCoroutine(currentPointerCoroutine);
                }
                currentPointerCoroutine = StartCoroutine(AnimatePointerToCenter());
            }
        }

        public void RemovePointerAnimation()
        {
        }

        public void MarkThisAnimation()
        {
            if (nodeAnimator != null && !string.IsNullOrEmpty(markApplyAnimationName))
            {
                nodeAnimator.Play(markApplyAnimationName);
            }
        }

        public void RemoveMarkAnimation()
        {
            if (nodeAnimator != null && !string.IsNullOrEmpty(markRevertAnimationName))
            {
                nodeAnimator.Play(markRevertAnimationName);
            }
        }

        private System.Collections.IEnumerator AnimatePointerToCenter()
        {
            if (pointerObject == null) yield break;

            Vector3 startPosition = pointerObject.position;
            Vector3 targetPosition = transform.position;
            float elapsedTime = 0f;

            while (elapsedTime < pointerAnimationDuration)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / pointerAnimationDuration;
                pointerObject.position = Vector3.Lerp(startPosition, targetPosition, t);
                yield return null;
            }

            pointerObject.position = targetPosition;
        }
    }
}

