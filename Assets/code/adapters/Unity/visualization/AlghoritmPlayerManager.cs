using Domain;
using GraphMaster.UnityAdapter.UI;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GraphMaster.UnityAdapter.Visualization
{
    public class AlghoritmPlayerManager : MonoBehaviour
    {
        [SerializeField] private GraphUI graphUI;
        [SerializeField] private Cursor cursor;
        [SerializeField] private RootExceptionHandler rootExceptionHandler;

        [SerializeField] private UnityEvent onVisualizationStart;
        [SerializeField] private UnityEvent onVisualizationStop;

        [SerializeField] private Color pathColor;

        private Coroutine currentActionsCoroutine = null;

        private DepthFirstSearchService<NodeUI, EdgeUI> dfsService = new DepthFirstSearchService<NodeUI, EdgeUI>();
        private BreadthFirstSearchService<NodeUI, EdgeUI> bfsService = new BreadthFirstSearchService<NodeUI, EdgeUI>();
        private DijkstraVisualisationService<NodeUI, EdgeUI> dijkstraService = new DijkstraVisualisationService<NodeUI, EdgeUI>();
        private HierholzerService<NodeUI, EdgeUI> heirholzerService = new HierholzerService<NodeUI, EdgeUI>();
        private BacktrackingHamiltonianCycleService<NodeUI, EdgeUI> backtrackingService = new BacktrackingHamiltonianCycleService<NodeUI, EdgeUI>();

        private void Awake()
        {

            if (cursor == null)
            {
                GameObject cursorObject = GameObject.FindGameObjectWithTag("Cursor");
                if (cursorObject != null)
                    cursor = cursorObject.GetComponent<Cursor>();
                cursor.OnAllActionsComplete += OnVisualisationEnd;
            }

            if(rootExceptionHandler == null)
            {
                throw new Exception(" Not root Exception handler");
            }
        }

        public void StartDepthFirstSearch()
        {
            if (!ValidateComponents()) return;

            Graph<NodeUI, EdgeUI> graph = graphUI.GetGraph();
            if (!graph.HasNodes()) return;

            List<ActionInterface> actions = dfsService.Traverse(graph);

            this.StartVisualisation(actions);

        }

        private void OnVisualisationEnd()
        {
            onVisualizationStop.Invoke();
        }

        public void StartBreadthFirstSearch()
        {
            if (!ValidateComponents()) return;

            Graph<NodeUI, EdgeUI> graph = graphUI.GetGraph();
            if (!graph.HasNodes()) return;

            List<ActionInterface> actions = bfsService.Traverse(graph);
            this.StartVisualisation(actions);
        }

        public void StartDijkstra()
        {
            if (!ValidateComponents()) return;

            Graph<NodeUI, EdgeUI> graph = graphUI.GetGraph();
            if (!graph.HasNodes()) return;

            List<ActionInterface> actions = dijkstraService.MakeDijkstra(graph);
            this.StartVisualisation(actions);
        }

        public void StartHierholzer()
        {
            if (!ValidateComponents()) return;

            Graph<NodeUI, EdgeUI> graph = graphUI.GetGraph();
            if (!graph.HasNodes()) return;

            try
            {
                List<ActionInterface> actions = heirholzerService.FindEulerianCycle(graph);
                this.StartVisualisation(actions);
            }
            catch (Exception e)
            {
                Debug.Log(" The count is not Eulerian ");
                onVisualizationStart.Invoke();
                rootExceptionHandler?.PublishError?.Invoke(e);
                OnVisualisationEnd();
            }
        }

        public void StartBacktracking()
        {
            if (!ValidateComponents()) return;

            Graph<NodeUI, EdgeUI> graph = graphUI.GetGraph();
            if (!graph.HasNodes()) return;

            try
            {
                List<ActionInterface> actions = backtrackingService.FindHamiltonianPath(graph);
                this.StartVisualisation(actions);
            }
            catch (Exception e)
            {
                onVisualizationStart.Invoke();
                rootExceptionHandler?.PublishError?.Invoke(e);
                OnVisualisationEnd();
            }
        }

        public void ClearVisualization()
        {
            cursor.ClearAll();
        }

        private bool ValidateComponents()
        {
            if (graphUI == null ||  cursor == null)
                return false;
            return true;
        }

        public void StopVisualisation()
        {
            if(currentActionsCoroutine != null)
            {
                StopCoroutine(currentActionsCoroutine);
            }
            
            currentActionsCoroutine = null;
            ClearVisualization();
        }


        private void StartVisualisation(List<ActionInterface> actions)
        {
            if (!ValidateComponents())
            {
                Debug.Log(" Not all elements are initialized");
                return;
            }

            if(currentActionsCoroutine != null)
            {
                StopCoroutine(currentActionsCoroutine);
            }
            onVisualizationStart.Invoke();
            currentActionsCoroutine = StartCoroutine(cursor.ExecuteActions(actions));

        }

        private void OnValidate()
        {
            this.heirholzerService.SetCircuitColor(ToSystemColor(pathColor));
        }

        public static System.Drawing.Color ToSystemColor(UnityEngine.Color unityColor)
        {
            return System.Drawing.Color.FromArgb(
                (int)(unityColor.a * 255),
                (int)(unityColor.r * 255),
                (int)(unityColor.g * 255),
                (int)(unityColor.b * 255)
            );
        }

    }
}
