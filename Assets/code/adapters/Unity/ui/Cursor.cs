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
        currentMovementCoroutine = StartCoroutine(SmoothMoveRoutine(returnDuration, startPosition));
    }

    public void MarkObject(GraphObjectUiActionsInterface graphObject)
    {
        if (currentMovementCoroutine != null)
        {
            StopCoroutine(currentMovementCoroutine);
        }

        currentMovementCoroutine = StartCoroutine(SmoothHarassmentRoutine(moveDuration, graphObject));
    }

    private void onCursorAnimationComplete()
    {
        whitingTheAnimation = false;
    }


    private IEnumerator SmoothMoveRoutine(float duration, Vector3 target)
    {
        Vector3 initialPosition = transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float remainingTime = duration - elapsedTime;


            float t = Mathf.Clamp01(elapsedTime / duration);
            float smoothT = t * t * (3f - 2f * t);

            transform.position = Vector3.Lerp(initialPosition, target, smoothT);
            yield return null;
        }

        transform.position = target;

        if (cursorAnimationHandler != null)
        {
            while (whitingTheAnimation)
            {
                yield return null;
            }
        }

        currentMovementCoroutine = null;

        OnMovementComplete?.Invoke();
    }
    private IEnumerator SmoothHarassmentRoutine(float duration, GraphObjectUiActionsInterface objectToMark)
    {
        Vector3 initialPosition = transform.position;
        float elapsedTime = 0f;
        bool animationTriggered = false;
        Vector3 target = objectToMark.GetCenterPosition();

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float remainingTime = duration - elapsedTime;
            
            if (!animationTriggered && objectToMark != null && remainingTime <= pointAnimationDuration)
            {
                if (animator != null)
                {
                    animator.SetTrigger("Point");
                }
                whitingTheAnimation = true;
                animationTriggered = true;
            }
            
            float t = Mathf.Clamp01(elapsedTime / duration);
            float smoothT = t * t * (3f - 2f * t);
            
            target = objectToMark.GetCenterPosition();
            transform.position = Vector3.Lerp(initialPosition, target, smoothT);
            yield return null;
        }

        transform.position = objectToMark.GetCenterPosition();

        if (cursorAnimationHandler != null)
        {
            while (whitingTheAnimation)
            {
                transform.position = objectToMark.GetCenterPosition();
                yield return null;
            }
        }

        if (objectToMark != null)
        {
            objectToMark.MarkThis();
        }

        currentMovementCoroutine = null;

        OnMovementComplete?.Invoke();
    }
}
