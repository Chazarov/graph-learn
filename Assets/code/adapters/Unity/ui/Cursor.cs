using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cursor : MonoBehaviour
{
    private Vector3 startPosition;

    [SerializeField] [Tooltip("Точное время перемещения к объекту (в секундах)")]
    private float moveDuration = 0.5f;
    
    [SerializeField] [Tooltip("Точное время возврата на начальную позицию (в секундах)")]
    private float returnDuration = 0.5f;
    
    [SerializeField] private Animator animator;
    [SerializeField] private float pointAnimationDuration = 1;

    private Coroutine currentMovementCoroutine;
    [SerializeField] private CursorAnimationHandler cursorAnimationHandler;

    /// <summary>
    /// Событие, вызываемое когда курсор завершил движение и отметил объект.
    /// </summary>
    public event Action OnMovementComplete;
    private bool whitingTheAnimation = false;


    void Start()
    {
        startPosition = transform.position;
        if (animator != null)
        {
            if(cursorAnimationHandler == null)
            {
                cursorAnimationHandler = animator.GetBehaviour<CursorAnimationHandler>();
            }
            else
            {
                Debug.LogWarning(" Сursor animation handler can't be a null. Please add this behavior for the animation to work correctly.");
            }
            
            cursorAnimationHandler.OnPointAnimationComplete += onCursorAnimationComplete;
        }
        else
        {
            Debug.LogWarning(" Add animator to the cursor object for animation processing");
        }
    }

    public void BackToStart()
    {
        if (currentMovementCoroutine != null)
        {
            StopCoroutine(currentMovementCoroutine);
        }
        currentMovementCoroutine = StartCoroutine(SmoothMoveRoutine(startPosition, returnDuration, null));
    }

    public void MarkObject(GraphObjectUiActionsInterface graphObject)
    {
        if (currentMovementCoroutine != null)
        {
            StopCoroutine(currentMovementCoroutine);
        }

        Vector3 targetPosition = graphObject.GetCenterPosition();
        currentMovementCoroutine = StartCoroutine(SmoothMoveRoutine(targetPosition, moveDuration, graphObject));
    }

    private void onCursorAnimationComplete()
    {
        whitingTheAnimation = false;
    }

    /// <summary>
    /// Универсальная корутина плавного перемещения за фиксированное время.
    /// </summary>
    /// <param name="targetPosition">Целевая позиция</param>
    /// <param name="duration">Точное время перемещения в секундах</param>
    /// <param name="objectToMark">Объект для маркировки после перемещения (null если не нужно)</param>
    private IEnumerator SmoothMoveRoutine(Vector3 targetPosition, float duration, GraphObjectUiActionsInterface objectToMark)
    {
        Vector3 initialPosition = transform.position;
        float elapsedTime = 0f;
        bool animationTriggered = false;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float remainingTime = duration - elapsedTime;
            
            // Запускаем анимацию Point ровно один раз, когда оставшееся время <= длительности анимации
            if (!animationTriggered && objectToMark != null && remainingTime <= pointAnimationDuration)
            {
                if (animator != null)
                {
                    animator.SetTrigger("Point");
                }
                whitingTheAnimation = true;
                animationTriggered = true;
            }
            
            // SmoothStep для плавного ускорения в начале и замедления в конце
            float t = Mathf.Clamp01(elapsedTime / duration);
            float smoothT = t * t * (3f - 2f * t);
            
            transform.position = Vector3.Lerp(initialPosition, targetPosition, smoothT);
            yield return null;
        }

        transform.position = targetPosition;

        if (cursorAnimationHandler != null)
        {
            while (whitingTheAnimation)
            {
                Debug.Log("Whiting the animation");
                yield return null;
            }
        }
        Debug.Log("End the animation");

        if (objectToMark != null)
        {
            objectToMark.MarkThis();
        }

        currentMovementCoroutine = null;

        // Вызываем событие завершения движения
        Debug.Log("End the action");
        OnMovementComplete?.Invoke();
    }
}
