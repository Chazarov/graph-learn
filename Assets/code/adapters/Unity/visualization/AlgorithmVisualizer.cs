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
        [SerializeField] private float stepDelay = 0.5f;

        private Cursor cursor;
        private Coroutine currentVisualizationCoroutine;
        private List<GraphObjectUiActionsInterface> allProcessedObjects = new List<GraphObjectUiActionsInterface>();

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

        public void Visualize(List<GraphObjectUiActionsInterface> objectsToMark)
        {

            Debug.Log(" Visulize");
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

                // Вызываем MarkObject у курсора, передавая текущий объект
                cursor.MarkObject(currentObject);
                allProcessedObjects.Add(currentObject);

                yield return new WaitForSeconds(stepDelay);
            }

            // Возвращаем курсор на начальную позицию после завершения
            cursor.BackToStart();

            currentVisualizationCoroutine = null;
            onVisualizationEnd?.Invoke();
        }
    }
}


