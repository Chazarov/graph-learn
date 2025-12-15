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
        private Dictionary<string, float> distances;
        private List<NodeUI> visualizedNodes = new List<NodeUI>();
        private string infinitySymbol = "∞";

        public DijkstraVisualizer(GraphInterface<NodeUI, EdgeUI> graph, Dictionary<string, float> distances)
        {
            this.graph = graph;
            this.distances = distances;
        }

        public IEnumerator StartVisualisation()
        {
            foreach (var pair in distances)
            {
                NodeUI node = graph.GetNode(pair.Key);
                if (node != null)
                {
                    if (pair.Value >= float.MaxValue)
                        node.VisualEffects.AdditionalValueController.ShowValue(infinitySymbol);
                    else
                        node.VisualEffects.AdditionalValueController.ShowValue(pair.Value);
                    
                    node.MarkThis();
                    visualizedNodes.Add(node);
                    yield return null;
                }
            }
        }


        public void ClearVisualisation()
        {
            foreach (var node in visualizedNodes)
            {
                if (node != null)
                {
                    node.VisualEffects.AdditionalValueController.HideValue();
                    node.RemoveMark();
                }
            }
            visualizedNodes.Clear();
        }
    }
}
