using UnityEngine;
using UnityEngine.EventSystems;

namespace GraphMaster.UnityAdapter.UI
{
    public class ButtonAnimation : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private float pressScale = 0.9f;
        [SerializeField] private float animationSpeed = 10f;

        private Vector3 originalScale;
        private Vector3 targetScale;

        void Start()
        {
            originalScale = transform.localScale;
            targetScale = originalScale;
        }

        void Update()
        {
            transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * animationSpeed);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            targetScale = originalScale * pressScale;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            targetScale = originalScale;
        }
    }
}

