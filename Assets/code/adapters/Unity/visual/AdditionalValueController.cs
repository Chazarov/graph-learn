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
        private bool isVisible = false;

        public string Value => value;

        public bool IsVisible => isVisible;

        private void Awake()
        {
            if (additionalValueToolbar != null)
            {
                initialToolbarScale = additionalValueToolbar.transform.localScale;
            }
        }

        /// <summary>
        /// Показывает тулбар с указанным строковым значением.
        /// Запускает анимацию появления через аниматор.
        /// </summary>
        /// <param name="newValue">Значение для отображения</param>
        public void ShowValue(string newValue)
        {
            if (string.IsNullOrEmpty(newValue)) return;

            value = newValue;
            UpdateValueText();

            // Если тулбар ещё не видим - запускаем анимацию появления
            if (!isVisible)
            {
                additionalValuesAnimator?.SetTrigger("Show");
                isVisible = true;
            }
            else
            {
                // Если уже видим - просто обновляем значение с анимацией
                additionalValuesAnimator?.SetTrigger("SetValue");
            }
        }

        /// <summary>
        /// Показывает тулбар с числовым значением.
        /// Масштабирует размер тулбара пропорционально значению.
        /// </summary>
        /// <param name="newValue">Числовое значение для отображения</param>
        public void ShowValue(float newValue)
        {
            value = newValue.ToString();
            UpdateValueText();

            // Если тулбар ещё не видим - запускаем анимацию появления
            if (!isVisible)
            {
                additionalValuesAnimator?.SetTrigger("Show");
                isVisible = true;
            }
            else
            {
                additionalValuesAnimator?.SetTrigger("SetValue");
            }

            // Масштабируем тулбар пропорционально значению
            SetToolbarScaleByValue(newValue);
        }

        /// <summary>
        /// Обновляет значение без анимации появления (если тулбар уже видим).
        /// </summary>
        /// <param name="newValue">Новое строковое значение</param>
        public void UpdateValue(string newValue)
        {
            if (!isVisible) 
            {
                ShowValue(newValue);
                return;
            }

            value = newValue;
            UpdateValueText();
            additionalValuesAnimator?.SetTrigger("SetValue");

            // Проверяем, является ли значение числом для масштабирования
            if (float.TryParse(newValue, out float floatValue))
            {
                SetToolbarScaleByValue(floatValue);
            }
        }

        /// <summary>
        /// Обновляет числовое значение с масштабированием тулбара.
        /// </summary>
        /// <param name="newValue">Новое числовое значение</param>
        public void UpdateValue(float newValue)
        {
            if (!isVisible)
            {
                ShowValue(newValue);
                return;
            }

            value = newValue.ToString();
            UpdateValueText();
            additionalValuesAnimator?.SetTrigger("SetValue");
            SetToolbarScaleByValue(newValue);
        }

        /// <summary>
        /// Скрывает тулбар и удаляет значение.
        /// Запускает анимацию исчезновения через аниматор.
        /// </summary>
        public void HideValue()
        {
            if (!isVisible) return;

            additionalValuesAnimator?.SetTrigger("Hide");
            isVisible = false;
            value = null;

            // Сбрасываем масштаб к начальному значению
            ResetToolbarScale();
        }

        /// <summary>
        /// Удаляет значение (алиас для HideValue).
        /// </summary>
        public void RemoveValue()
        {
            HideValue();
        }

        /// <summary>
        /// Устанавливает масштаб тулбара пропорционально переданному значению.
        /// Использует линейную интерполяцию между minValueToolbarScale и maxValueToolbarScale.
        /// </summary>
        /// <param name="currentValue">Текущее значение</param>
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
        }

        /// <summary>
        /// Сбрасывает масштаб тулбара к начальному значению.
        /// </summary>
        private void ResetToolbarScale()
        {
            if (additionalValueToolbar != null)
            {
                additionalValueToolbar.transform.localScale = initialToolbarScale;
            }
        }

        /// <summary>
        /// Обновляет текстовое отображение значения.
        /// </summary>
        private void UpdateValueText()
        {
            if (valueText != null)
            {
                valueText.text = value ?? string.Empty;
            }
        }

        /// <summary>
        /// Проверяет корректность настройки компонента.
        /// </summary>
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

