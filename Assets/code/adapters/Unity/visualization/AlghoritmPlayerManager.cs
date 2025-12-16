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
        private DijkstraVisualisationService<NodeUI, EdgeUI> dijkstraService;

        private void Awake()
        {
            dfsService = new DepthFirstSearchService<NodeUI, EdgeUI>();
            bfsService = new BreadthFirstSearchService<NodeUI, EdgeUI>();
            dijkstraService = new DijkstraVisualisationService<NodeUI, EdgeUI>();

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

            List<ActionInterface> actions = dfsService.Traverse(graph);
            var breadthDepthVisualizer = new BreadthDepthVisualizer(actions, cursor);
            visualizer.StartVisualisation(breadthDepthVisualizer);
        }

        public void StartBreadthFirstSearch()
        {
            if (!ValidateComponents()) return;

            Graph<NodeUI, EdgeUI> graph = graphUI.GetGraph();
            if (!graph.HasNodes()) return;

            List<ActionInterface> actions = bfsService.Traverse(graph);
            var breadthDepthVisualizer = new BreadthDepthVisualizer(actions, cursor);
            visualizer.StartVisualisation(breadthDepthVisualizer);
        }

        public void StartDijkstra()
        {
            if (!ValidateComponents()) return;

            Graph<NodeUI, EdgeUI> graph = graphUI.GetGraph();
            if (!graph.HasNodes()) return;

            List<ActionInterface> actions = dijkstraService.MakeDijkstra(graph);
            var dijkstraVisualizer = new DijkstraVisualizer(graph, actions, cursor);
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
    }
}
