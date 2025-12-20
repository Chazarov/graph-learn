using Domain;
using GraphMaster.Visualization.Actions;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace GraphMaster
{
    public class LargestFirstColoringService<TNode, TEdge>
        where TNode : GraphNodeInterface, GraphPartInterface
        where TEdge : GraphEdgeInterface<TNode>, GraphPartInterface
    {
        private List<Color> pastelColors = new List<Color>
        {
            Color.FromArgb(255, 179, 186),
            Color.FromArgb(255, 223, 186),
            Color.FromArgb(255, 255, 186),
            Color.FromArgb(186, 255, 201),
            Color.FromArgb(186, 225, 255),
            Color.FromArgb(218, 186, 255),
            Color.FromArgb(255, 186, 255),
            Color.FromArgb(186, 255, 255),
            Color.FromArgb(255, 204, 229),
            Color.FromArgb(204, 255, 229),
            Color.FromArgb(229, 204, 255),
            Color.FromArgb(255, 229, 204),
            Color.FromArgb(204, 229, 255),
            Color.FromArgb(229, 255, 204),
            Color.FromArgb(255, 218, 185),
            Color.FromArgb(221, 160, 221)
        };

        public List<ActionInterface> ColorGraph(GraphInterface<TNode, TEdge> graph)
        {
            List<ActionInterface> actions = new();

            if (!graph.HasNodes())
                return actions;

            var nodes = graph.GetNodes();
            var edges = graph.GetEdges();
            bool isDirected = graph.GetIsDirected();

            Dictionary<string, int> degree = new();
            Dictionary<string, List<string>> neighbors = new();

            foreach (var node in nodes)
            {
                string name = node.GetName();
                degree[name] = 0;
                neighbors[name] = new List<string>();
            }

            foreach (var edge in edges)
            {
                string source = edge.GetSourseName();
                string target = edge.GetTargetName();

                degree[source]++;
                neighbors[source].Add(target);

                if (!isDirected)
                {
                    degree[target]++;
                    neighbors[target].Add(source);
                }
            }

            var sortedNodes = nodes.OrderByDescending(n => degree[n.GetName()]).ToList();

            Dictionary<string, int> nodeColors = new();

            foreach (var node in sortedNodes)
            {
                string nodeName = node.GetName();


                HashSet<int> usedColors = new();
                foreach (var neighborName in neighbors[nodeName])
                {
                    if (nodeColors.ContainsKey(neighborName))
                    {
                        usedColors.Add(nodeColors[neighborName]);
                    }
                }

                int colorIndex = 0;
                while (usedColors.Contains(colorIndex))
                {
                    colorIndex++;
                }

                nodeColors[nodeName] = colorIndex;

                Color nodeColor = GetColor(colorIndex);
                actions.Add(new SetColorAction(node, nodeColor));
                actions.Add(new SetAdditionalValue((colorIndex + 1).ToString(), node));
            }

            return actions;
        }

        private Color GetColor(int index)
        {
            if (index < pastelColors.Count)
                return pastelColors[index];

            int r = 180 + (index * 37) % 75;
            int g = 180 + (index * 53) % 75;
            int b = 180 + (index * 71) % 75;
            return Color.FromArgb(r, g, b);
        }
    }
}

