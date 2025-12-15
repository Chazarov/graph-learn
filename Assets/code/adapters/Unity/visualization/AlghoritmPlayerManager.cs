using Domain;
using GraphMaster.UnityAdapter.UI;
using System.Collections.Generic;
using UnityEngine;

namespace GraphMaster.UnityAdapter.Visualization
{
    public class AlghoritmPlayerManager : MonoBehaviour
    {
        [SerializeField] private GraphUI graphUI;
        [SerializeField] private AlgorithmVisualizer visualizer;
        [SerializeField] private Cursor cursor;

        private DepthFirstSearchService<NodeUI, EdgeUI> dfsService;
        private BreadthFirstSearchService<NodeUI, EdgeUI> bfsService;
        private DijkstraService<NodeUI, EdgeUI> dijkstraService;

        private void Awake()
        {
            dfsService = new DepthFirstSearchService<NodeUI, EdgeUI>();
            bfsService = new BreadthFirstSearchService<NodeUI, EdgeUI>();
            dijkstraService = new DijkstraService<NodeUI, EdgeUI>();

            if (cursor == null)
            {
                GameObject cursorObject = GameObject.FindGameObjectWithTag("Cursor");
                if (cursorObject != null)
                    cursor = cursorObject.GetComponent<Cursor>();
            }
        }

        public void StartDepthFirstSearch()
        {
            if (!ValidateComponents()) return;

            Graph<NodeUI, EdgeUI> graph = graphUI.GetGraph();
            if (!graph.HasNodes()) return;

            List<GraphPartInterface> traversalResult = dfsService.Traverse(graph);
            List<GraphObjectUiActionsInterface> objects = ConvertToUiObjects(traversalResult);

            var breadthDepthVisualizer = new BreadthDepthVisualizer(objects, cursor);
            visualizer.StartVisualisation(breadthDepthVisualizer);
        }

        public void StartBreadthFirstSearch()
        {
            if (!ValidateComponents()) return;

            Graph<NodeUI, EdgeUI> graph = graphUI.GetGraph();
            if (!graph.HasNodes()) return;

            List<GraphPartInterface> traversalResult = bfsService.Traverse(graph);
            List<GraphObjectUiActionsInterface> objects = ConvertToUiObjects(traversalResult);

            var breadthDepthVisualizer = new BreadthDepthVisualizer(objects, cursor);
            visualizer.StartVisualisation(breadthDepthVisualizer);
        }

        public void StartDijkstra()
        {
            if (!ValidateComponents()) return;

            Graph<NodeUI, EdgeUI> graph = graphUI.GetGraph();
            if (!graph.HasNodes()) return;

            Dictionary<string, float> distances = dijkstraService.FindShortestPaths(graph);

            var dijkstraVisualizer = new DijkstraVisualizer(graph, distances);
            visualizer.StartVisualisation(dijkstraVisualizer);
        }


        public void ClearVisualization()
        {
            visualizer?.ClearVisualisation();
        }

        private bool ValidateComponents()
        {
            if (graphUI == null || visualizer == null || cursor == null)
                return false;
            return true;
        }

        private List<GraphObjectUiActionsInterface> ConvertToUiObjects(List<GraphPartInterface> parts)
        {
            List<GraphObjectUiActionsInterface> result = new List<GraphObjectUiActionsInterface>();
            foreach (var part in parts)
            {
                if (part is GraphObjectUiActionsInterface uiObject)
                    result.Add(uiObject);
            }
            return result;
        }
    }
}
