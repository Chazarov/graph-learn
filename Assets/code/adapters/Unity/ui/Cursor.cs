using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cursor : MonoBehaviour
{
    private Vector3 startPosition;

    [SerializeField] private float returnDuration = 0.5f;
    [SerializeField] private float moveDuration = 1.0f;
    [SerializeField] private Animator animator;

    private Coroutine currentMovementCoroutine;

    void Start()
    {
        startPosition = transform.position;
    }

    public void BackToStart()
    {
        if (currentMovementCoroutine != null)
        {
            StopCoroutine(currentMovementCoroutine);
        }
        currentMovementCoroutine = StartCoroutine(MoveToPositionRoutine(startPosition));
    }

    private IEnumerator MoveToPositionRoutine(Vector3 targetPosition)
    {
        Vector3 initialPosition = transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < returnDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / returnDuration;
            
            // Используем SmoothStep для плавного ускорения и замедления
            float smoothT = t * t * (3f - 2f * t);
            
            transform.position = Vector3.Lerp(initialPosition, targetPosition, smoothT);
            yield return null;
        }

        transform.position = targetPosition;
        currentMovementCoroutine = null;
    }

    public void MarkObject(GraphObjectUiActionsInterface graphObject)
    {
        if (currentMovementCoroutine != null)
        {
            StopCoroutine(currentMovementCoroutine);
        }

        Debug.Log("Cursor Mark object");
        Vector3 targetPosition = graphObject.GetCenterPosition();
        currentMovementCoroutine = StartCoroutine(MoveAndMarkRoutine(targetPosition, graphObject));
    }

    private IEnumerator MoveAndMarkRoutine(Vector3 targetPosition, GraphObjectUiActionsInterface graphObject)
    {
        Vector3 initialPosition = transform.position;
        float elapsedTime = 0f;

        Debug.Log(" Move And Mark Routine Start");

        while (elapsedTime < returnDuration)
        {
            Debug.Log(" Move And Mark Routine Work");
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / moveDuration;
            
            float smoothT = t * t * (3f - 2f * t);
            
            transform.position = Vector3.Lerp(initialPosition, targetPosition, smoothT);
            yield return null;
        }

        transform.position = targetPosition;

        // Запускаем триггер Point в аниматоре
        if (animator != null)
        {
            animator.SetTrigger("Point");
        }
        Debug.Log(" Move And Mark Routine End");
        // Вызываем MarkThis у объекта
        graphObject.MarkThis();

        currentMovementCoroutine = null;
    }
}
