using Domain;
using System.Collections.Generic;

namespace GraphMaster
{
    public class EulerianCheckService<TNode, TEdge>
        where TNode : GraphNodeInterface, GraphPartInterface
        where TEdge : GraphEdgeInterface<TNode>, GraphPartInterface
    {
        public void CheckEulerian(GraphInterface<TNode, TEdge> graph)
        {
            if (!graph.HasNodes())
                throw new NotEulerianGraphException("Граф пуст");

            var nodes = graph.GetNodes();
            var edges = graph.GetEdges();
            bool isDirected = graph.GetIsDirected();

            Dictionary<string, int> inDegree = new();
            Dictionary<string, int> outDegree = new();

            foreach (var node in nodes)
            {
                inDegree[node.GetName()] = 0;
                outDegree[node.GetName()] = 0;
            }

            foreach (var edge in edges)
            {
                string source = edge.GetSourseName();
                string target = edge.GetTargetName();

                outDegree[source]++;
                inDegree[target]++;

                if (!isDirected)
                {
                    outDegree[target]++;
                    inDegree[source]++;
                }
            }

            if (isDirected)
            {
                foreach (var node in nodes)
                {
                    string name = node.GetName();
                    if (inDegree[name] != outDegree[name])
                        throw new NotEulerianGraphException("Для ориентированного графа: входящая степень должна равняться исходящей для всех вершин");
                }
            }
            else
            {
                foreach (var node in nodes)
                {
                    string name = node.GetName();
                    if (outDegree[name] % 2 != 0)
                        throw new NotEulerianGraphException("Для неориентированного графа: все вершины должны иметь чётную степень");
                }
            }

            if (!IsConnected(graph, nodes, edges, isDirected))
                throw new NotEulerianGraphException("Граф не связный");
        }

        private bool IsConnected(GraphInterface<TNode, TEdge> graph, List<TNode> nodes, List<TEdge> edges, bool isDirected)
        {
            if (nodes.Count == 0) return true;
            if (edges.Count == 0 && nodes.Count > 1) return false;

            HashSet<string> visited = new();
            Queue<string> queue = new();

            string start = nodes[0].GetName();
            queue.Enqueue(start);
            visited.Add(start);

            Dictionary<string, List<string>> adj = new();
            foreach (var node in nodes)
                adj[node.GetName()] = new List<string>();

            foreach (var edge in edges)
            {
                string source = edge.GetSourseName();
                string target = edge.GetTargetName();
                adj[source].Add(target);
                if (!isDirected)
                    adj[target].Add(source);
            }

            while (queue.Count > 0)
            {
                string current = queue.Dequeue();
                foreach (var neighbor in adj[current])
                {
                    if (!visited.Contains(neighbor))
                    {
                        visited.Add(neighbor);
                        queue.Enqueue(neighbor);
                    }
                }
            }

            foreach (var node in nodes)
            {
                string name = node.GetName();
                bool hasEdges = false;
                foreach (var edge in edges)
                {
                    if (edge.GetSourseName() == name || edge.GetTargetName() == name)
                    {
                        hasEdges = true;
                        break;
                    }
                }
                if (hasEdges && !visited.Contains(name))
                    return false;
            }

            return true;
        }
    }
}

