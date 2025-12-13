using Domain;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GraphMaster.UnityAdapter.Visualization
{
    public class AlgorithmVisualizer : MonoBehaviour
    {
        [SerializeField] private UnityEvent onVisualizationStart;
        [SerializeField] private UnityEvent onVisualizationEnd;

        private Cursor cursor;
        private Coroutine currentVisualizationCoroutine;
        private List<GraphObjectUiActionsInterface> allProcessedObjects = new List<GraphObjectUiActionsInterface>();
        
        private bool waitingForCursor = false;

        private void Awake()
        {
            GameObject cursorObject = GameObject.FindGameObjectWithTag("Cursor");
            if (cursorObject != null)
            {
                cursor = cursorObject.GetComponent<Cursor>();
            }

            if (cursor == null)
            {
                Debug.LogError("Cursor не найден! Убедитесь, что объект с компонентом Cursor имеет тег 'Cursor'.");
            }
        }

        private void OnEnable()
        {
            if (cursor != null)
            {
                cursor.OnMovementComplete += OnCursorMovementComplete;
            }
        }

        private void OnDisable()
        {
            if (cursor != null)
            {
                cursor.OnMovementComplete -= OnCursorMovementComplete;
            }
        }

        private void OnCursorMovementComplete()
        {
            waitingForCursor = false;
        }

        public void Visualize(List<GraphObjectUiActionsInterface> objectsToMark)
        {
            if (cursor == null)
            {
                Debug.LogError("Невозможно запустить визуализацию: Cursor не найден.");
                return;
            }

            if (currentVisualizationCoroutine != null)
            {
                StopCoroutine(currentVisualizationCoroutine);
            }
            currentVisualizationCoroutine = StartCoroutine(VisualizationRoutine(objectsToMark));
        }

        public void Clear()
        {
            foreach (var obj in allProcessedObjects)
            {
                obj.RemoveMark();
            }
            allProcessedObjects.Clear();

            cursor?.BackToStart();
        }

        public void Stop()
        {
            if (currentVisualizationCoroutine != null)
            {
                StopCoroutine(currentVisualizationCoroutine);
                currentVisualizationCoroutine = null;
                waitingForCursor = false;
                cursor?.BackToStart();
                onVisualizationEnd?.Invoke();
            }
        }

        private IEnumerator VisualizationRoutine(List<GraphObjectUiActionsInterface> objectsToMark)
        {
            onVisualizationStart?.Invoke();

            for (int i = 0; i < objectsToMark.Count; i++)
            {
                var currentObject = objectsToMark[i];

                waitingForCursor = true;

                cursor.MarkObject(currentObject);
                allProcessedObjects.Add(currentObject);

                while (waitingForCursor)
                {
                    yield return null;
                }
            }

            waitingForCursor = true;
            cursor.BackToStart();
            
            while (waitingForCursor)
            {
                yield return null;
            }

            currentVisualizationCoroutine = null;
            onVisualizationEnd?.Invoke();
        }
    }
}


