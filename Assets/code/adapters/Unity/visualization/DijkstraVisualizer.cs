using Domain;
using GraphMaster.UnityAdapter.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GraphMaster.UnityAdapter.Visualization
{
    public class DijkstraVisualizer : IAlgorithmVisualizer
    {
        private GraphInterface<NodeUI, EdgeUI> graph;
        private List<ActionInterface> actions;
        private Cursor cursor;

        public DijkstraVisualizer(GraphInterface<NodeUI, EdgeUI> graph, List<ActionInterface> actions, Cursor cursor)
        {
            this.graph = graph;
            this.actions = actions;
            this.cursor = cursor;
        }

        public IEnumerator StartVisualisation()
        {
            var executeCoroutine = cursor.ExecuteActions(actions);
            return executeCoroutine;
        }


        public void ClearVisualisation()
        {
            cursor.UnmarkAll();
            foreach (var node in graph.GetNodes())
            {
                if (node != null)
                {
                    node.VisualEffects.AdditionalValueController.RemoveValue();
                }
            }

        }

    }
}
