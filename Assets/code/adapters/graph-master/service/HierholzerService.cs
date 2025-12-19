using Domain;
using GraphMaster.Visualization.Actions;
using System.Collections.Generic;
using System.Drawing;

namespace GraphMaster
{
    public class HierholzerService<TNode, TEdge>
        where TNode : GraphNodeInterface, GraphPartInterface
        where TEdge : GraphEdgeInterface<TNode>, GraphPartInterface
    {
        public Color circuitColor = Color.LimeGreen;
        private EulerianCheckService<TNode, TEdge> eulerianCheck = new();

        public List<ActionInterface> FindEulerianCycle(GraphInterface<TNode, TEdge> graph)
        {
            List<ActionInterface> actions = new();

            eulerianCheck.CheckEulerian(graph);

            bool isDirected = graph.GetIsDirected();

            Dictionary<string, List<TEdge>> availableEdges = new();
            HashSet<string> usedEdges = new();

            foreach (var edge in graph.GetEdges())
            {
                string source = edge.GetSourseName();
                string target = edge.GetTargetName();

                if (!availableEdges.ContainsKey(source))
                    availableEdges[source] = new List<TEdge>();
                availableEdges[source].Add(edge);

                if (!isDirected)
                {
                    if (!availableEdges.ContainsKey(target))
                        availableEdges[target] = new List<TEdge>();
                    availableEdges[target].Add(edge);
                }
            }

            TNode startNode = graph.GetRoot();
            string startName = startNode.GetName();

            Stack<string> currentPath = new();
            List<string> circuit = new();
            List<TEdge> circuitEdges = new();
            Dictionary<string, TEdge> pathEdges = new();

            currentPath.Push(startName);
            actions.Add(new MarkThis(startNode));

            while (currentPath.Count > 0)
            {
                string current = currentPath.Peek();

                bool foundEdge = false;
                int edgeIndex = -1;

                if (availableEdges.ContainsKey(current))
                {
                    var edges = availableEdges[current];
                    for (int i = 0; i < edges.Count; i++)
                    {
                        if (!usedEdges.Contains(edges[i].GetName()))
                        {
                            edgeIndex = i;
                            foundEdge = true;
                            break;
                        }
                    }
                }

                if (foundEdge)
                {
                    TEdge nextEdge = availableEdges[current][edgeIndex];
                    usedEdges.Add(nextEdge.GetName());

                    string nextNode = nextEdge.GetTargetName();
                    if (nextNode == current)
                        nextNode = nextEdge.GetSourseName();

                    actions.Add(new MarkThis(nextEdge));
                    actions.Add(new MarkThis(graph.GetNode(nextNode)));

                    pathEdges[current + "->" + nextNode] = nextEdge;
                    currentPath.Push(nextNode);
                }
                else
                {
                    string node = currentPath.Pop();
                    circuit.Add(node);

                    if (currentPath.Count > 0)
                    {
                        string prev = currentPath.Peek();
                        string key1 = prev + "->" + node;
                        string key2 = node + "->" + prev;
                        if (pathEdges.ContainsKey(key1))
                            circuitEdges.Add(pathEdges[key1]);
                        else if (pathEdges.ContainsKey(key2))
                            circuitEdges.Add(pathEdges[key2]);
                    }
                }
            }

            circuit.Reverse();
            circuitEdges.Reverse();

            Dictionary<string, string> usingNodes = new();

            for (int i = 0; i < circuit.Count; i++)
            {
                string nodeName = circuit[i];
                TNode node = graph.GetNode(circuit[i]);
                actions.Add(new SetColorAction(node, circuitColor));

                if (!usingNodes.ContainsKey(nodeName)) usingNodes[nodeName] = "'" + (i + 1).ToString();
                else usingNodes[nodeName] += "|" + (i + 1).ToString();

                actions.Add(new SetAdditionalValue(usingNodes[nodeName], node));

                if (i < circuitEdges.Count)
                {
                    actions.Add(new SetColorAction(circuitEdges[i], circuitColor));
                }
            }

            return actions;
        }

        public void SetCircuitColor(Color value)
        {
            this.circuitColor = value;
        }
    }
}
