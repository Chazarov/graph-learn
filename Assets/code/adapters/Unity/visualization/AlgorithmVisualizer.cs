using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace GraphMaster.UnityAdapter.Visualization
{
    public class AlgorithmVisualizer : MonoBehaviour
    {
        [SerializeField] private UnityEvent onVisualizationStart;
        [SerializeField] private UnityEvent onVisualizationStop;

        private Coroutine currentCoroutine;
        private IAlgorithmVisualizer currentVisualizer;
        public bool VisualizationIsRun => currentCoroutine != null;

        private void Start()
        {
            
        }

        public void StartVisualisation(IAlgorithmVisualizer visualizer)
        {
            ClearVisualisation();
            
            currentVisualizer = visualizer;
            onVisualizationStart?.Invoke();
            currentCoroutine = StartCoroutine(VisualizationRoutine());
        }


        public void ClearVisualisation()
        {
            currentVisualizer?.ClearVisualisation();
            currentVisualizer = null;
        }

        private IEnumerator VisualizationRoutine()
        {
            yield return currentVisualizer.StartVisualisation();
            
            currentCoroutine = null;
            onVisualizationStop?.Invoke();
        }
    }
}
