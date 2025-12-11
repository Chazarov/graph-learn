using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cursor : MonoBehaviour
{
    private Vector3 startPosition;

    [SerializeField] private float returnDuration = 0.5f;

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
}
