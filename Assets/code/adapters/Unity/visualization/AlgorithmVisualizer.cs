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
        [SerializeField] private UnityEvent onVisualizationPause;
        [SerializeField] private UnityEvent onVisualizationResume;
        [SerializeField] private UnityEvent onVisualizationCancel;
        [SerializeField] private float stepDelay = 0.5f;

        private Coroutine currentVisualizationCoroutine;
        private List<GraphObjectUiActionsInterface> allProcessedObjects = new List<GraphObjectUiActionsInterface>();
        private bool isPaused = false;

        public void Visualize(List<GraphObjectUiActionsInterface> objectsToPoint)
        {
            if (currentVisualizationCoroutine != null)
            {
                StopCoroutine(currentVisualizationCoroutine);
            }
            currentVisualizationCoroutine = StartCoroutine(VisualizationRoutine(objectsToPoint));
        }

        public void Clear()
        {
            foreach (var obj in allProcessedObjects)
            {
                obj.RemoveMark();
            }
            allProcessedObjects.Clear();
        }

        public void Stop()
        {
            if (currentVisualizationCoroutine != null)
            {
                StopCoroutine(currentVisualizationCoroutine);
                currentVisualizationCoroutine = null;
                isPaused = false;
                onVisualizationEnd?.Invoke();
            }
        }

        public void Pause()
        {
            if (currentVisualizationCoroutine != null && !isPaused)
            {
                isPaused = true;
                onVisualizationPause?.Invoke();
            }
        }

        public void Resume()
        {
            if (currentVisualizationCoroutine != null && isPaused)
            {
                isPaused = false;
                onVisualizationResume?.Invoke();
            }
        }

        public void Cancel()
        {
            if (currentVisualizationCoroutine != null)
            {
                StopCoroutine(currentVisualizationCoroutine);
                currentVisualizationCoroutine = null;
                isPaused = false;
                Clear();
                onVisualizationCancel?.Invoke();
            }
        }

        private IEnumerator VisualizationRoutine(List<GraphObjectUiActionsInterface> objectsToPoint)
        {
            onVisualizationStart?.Invoke();

            GraphObjectUiActionsInterface previousPointed = null;

            for (int i = 0; i < objectsToPoint.Count; i++)
            {
                while (isPaused)
                {
                    yield return null;
                }

                var currentObject = objectsToPoint[i];

                if (previousPointed != null)
                {
                    previousPointed.RemovePointer();
                }

                currentObject.MarkThis();
                currentObject.PointThis();
                previousPointed = currentObject;
                allProcessedObjects.Add(currentObject);

                yield return new WaitForSeconds(stepDelay);
            }

            if (previousPointed != null)
            {
                previousPointed.RemovePointer();
            }

            currentVisualizationCoroutine = null;
            isPaused = false;
            onVisualizationEnd?.Invoke();
        }
    }
}


