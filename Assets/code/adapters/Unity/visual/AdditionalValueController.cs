using UnityEngine;
using TMPro;

namespace GraphMaster.UnityAdapter.VisualEffects
{
    public class AdditionalValueController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameObject additionalValueToolbar;
        [SerializeField] private TextMeshProUGUI valueText;
        [SerializeField] private Animator additionalValuesAnimator;

        [Header("Value Settings")]
        [SerializeField] private string value = null;

        [Header("Toolbar Scaling")]
        [Tooltip("Максимальный масштаб тулбара")]
        [SerializeField] private float maxValueToolbarScale = 3.0f;
        
        [Tooltip("Максимальное значение для масштабирования")]
        [SerializeField] private float maxValue = 10f;
        
        [Tooltip("Минимальный масштаб тулбара")]
        [SerializeField] private float minValueToolbarScale = 1.0f;
        
        [Tooltip("Минимальное значение для масштабирования")]
        [SerializeField] private float minValue = 0f;

        private Vector3 initialToolbarScale;
        private Vector3 initialToolbarPosition;
        private bool isVisible = false;

        public string Value => value;

        public bool IsVisible => isVisible;

        private void Start()
        {
            CheckConfiguration();
        }

        private void Awake()
        {
            if (additionalValueToolbar != null)
            {
                initialToolbarScale = additionalValueToolbar.transform.localScale;
                initialToolbarPosition = additionalValueToolbar.transform.localPosition;
            }
        }

        public void SetValue(string newValue)
        {
            if (string.IsNullOrEmpty(newValue)) return;

            value = newValue;
            if(float.TryParse(value, out float  fvalue))
            {
                this.SetToolbarScaleByValue(fvalue);
            }
            UpdateValueText();

            if (!isVisible)
            {
                additionalValuesAnimator?.SetBool("Show", true);
                isVisible = true;
            }
            else
            {
                additionalValuesAnimator?.SetTrigger("SetValue");
            }

            
        }

       



        public void RemoveValue()
        {
            if (!isVisible) return;

            additionalValuesAnimator?.SetBool("Show", false);
            isVisible = false;
            value = null;

            // Сбрасываем масштаб к начальному значению
            ResetToolbarScale();
        }


        private void SetToolbarScaleByValue(float currentValue)
        {
            if (additionalValueToolbar == null) return;

            // Нормализуем значение в диапазоне [0, 1]
            float normalizedValue = Mathf.InverseLerp(minValue, maxValue, currentValue);
            
            // Интерполируем масштаб между минимальным и максимальным
            float targetScale = Mathf.Lerp(minValueToolbarScale, maxValueToolbarScale, normalizedValue);

            // Применяем масштаб (сохраняем пропорции начального масштаба)
            Vector3 newScale = initialToolbarScale * targetScale;
            additionalValueToolbar.transform.localScale = newScale;

            // Смещаем позицию вверх, чтобы нижний край оставался на месте
            float heightDifference = newScale.y - initialToolbarScale.y;
            Vector3 newPosition = initialToolbarPosition;
            newPosition.y += heightDifference * 2;
            additionalValueToolbar.transform.localPosition = newPosition;
        }

        private void ResetToolbarScale()
        {
            if (additionalValueToolbar != null)
            {
                additionalValueToolbar.transform.localScale = initialToolbarScale;
                additionalValueToolbar.transform.localPosition = initialToolbarPosition;
            }
        }

        private void UpdateValueText()
        {
            if (valueText != null)
            {
                valueText.text = value ?? string.Empty;

                if (float.TryParse(value, out float fvalue))
                {
                    if (float.IsInfinity(fvalue) || fvalue >= float.MaxValue)
                    {
                        valueText.text = "∞";
                    }
                }
            }
        }

        public void CheckConfiguration()
        {
            if (additionalValueToolbar == null)
            {
                Debug.LogWarning($"[{nameof(AdditionalValueController)}] additionalValueToolbar не назначен!");
            }

            if (valueText == null)
            {
                Debug.LogWarning($"[{nameof(AdditionalValueController)}] valueText не назначен!");
            }

            if (additionalValuesAnimator == null)
            {
                Debug.LogWarning($"[{nameof(AdditionalValueController)}] additionalValuesAnimator не назначен!");
            }

            if (minValue >= maxValue)
            {
                Debug.LogWarning($"[{nameof(AdditionalValueController)}] minValue ({minValue}) должен быть меньше maxValue ({maxValue})!");
            }

            if (minValueToolbarScale >= maxValueToolbarScale)
            {
                Debug.LogWarning($"[{nameof(AdditionalValueController)}] minValueToolbarScale ({minValueToolbarScale}) должен быть меньше maxValueToolbarScale ({maxValueToolbarScale})!");
            }
        }
    }
}

