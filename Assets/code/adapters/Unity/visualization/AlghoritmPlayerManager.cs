using Domain;
using GraphMaster.UnityAdapter.UI;
using System.Collections.Generic;
using UnityEngine;

namespace GraphMaster.UnityAdapter.Visualization
{
    /// <summary>
    /// Менеджер для запуска и визуализации алгоритмов обхода графа.
    /// </summary>
    public class AlghoritmPlayerManager : MonoBehaviour
    {
        [SerializeField] private GraphUI graphUI;
        [SerializeField] private AlgorithmVisualizer visualizer;

        private DepthFirstSearchService<NodeUI, EdgeUI> dfsService;
        private BreadthFirstSearchService<NodeUI, EdgeUI> bfsService;

        private void Awake()
        {
            dfsService = new DepthFirstSearchService<NodeUI, EdgeUI>();
            bfsService = new BreadthFirstSearchService<NodeUI, EdgeUI>();
        }

        public void StartDepthFirstSearch()
        {
            if (!ValidateComponents()) return;

            Graph<NodeUI, EdgeUI> graph = graphUI.GetGraph();

            if (!graph.HasNodes())
            {
                Debug.LogWarning("Граф пуст. Добавьте узлы перед запуском алгоритма.");
                return;
            }

            visualizer.Clear();

            List<GraphPartInterface> traversalResult = dfsService.Traverse(graph);

            List<GraphObjectUiActionsInterface> objectsToVisualize = ConvertToUiObjects(traversalResult);

            visualizer.Visualize(objectsToVisualize);
        }

        public void StartBreadthFirstSearch()
        {
            if (!ValidateComponents()) return;

            Graph<NodeUI, EdgeUI> graph = graphUI.GetGraph();

            if (!graph.HasNodes())
            {
                Debug.LogWarning("Граф пуст. Добавьте узлы перед запуском алгоритма.");
                return;
            }

            visualizer.Clear();

            List<GraphPartInterface> traversalResult = bfsService.Traverse(graph);

            List<GraphObjectUiActionsInterface> objectsToVisualize = ConvertToUiObjects(traversalResult);

            visualizer.Visualize(objectsToVisualize);
        }

        private bool ValidateComponents()
        {
            if (graphUI == null)
            {
                Debug.LogError("GraphUI не назначен в AlghoritmPlayerManager.");
                return false;
            }

            if (visualizer == null)
            {
                Debug.LogError("AlgorithmVisualizer не назначен в AlghoritmPlayerManager.");
                return false;
            }

            return true;
        }

        private List<GraphObjectUiActionsInterface> ConvertToUiObjects(List<GraphPartInterface> parts)
        {
            List<GraphObjectUiActionsInterface> result = new List<GraphObjectUiActionsInterface>();

            foreach (var part in parts)
            {
                if (part is GraphObjectUiActionsInterface uiObject)
                {
                    result.Add(uiObject);
                }
            }

            return result;
        }
    }
}

