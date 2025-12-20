using Domain;
using System.Collections.Generic;

namespace GraphMaster
{
    public class HamiltonianCheckService<TNode, TEdge>
        where TNode : GraphNodeInterface, GraphPartInterface
        where TEdge : GraphEdgeInterface<TNode>, GraphPartInterface
    {
        public void CheckHamiltonian(GraphInterface<TNode, TEdge> graph)
        {
            if (!graph.HasNodes())
                throw new NotHamiltonianGraphException("Граф пуст");

            var nodes = graph.GetNodes();
            var edges = graph.GetEdges();

            if (nodes.Count < 3)
                throw new NotHamiltonianGraphException("Граф должен содержать минимум 3 вершины");

            bool isDirected = graph.GetIsDirected();
            Dictionary<string, int> degree = new();

            foreach (var node in nodes)
                degree[node.GetName()] = 0;

            foreach (var edge in edges)
            {
                string source = edge.GetSourseName();
                string target = edge.GetTargetName();

                degree[source]++;
                if (!isDirected)
                    degree[target]++;
            }

            foreach (var node in nodes)
            {
                string name = node.GetName();
                if (degree[name] < 2)
                    throw new NotHamiltonianGraphException("Все вершины должны иметь степень >= 2");
            }

            if (!Is2VertexConnected(graph, nodes, edges, isDirected))
                throw new NotHamiltonianGraphException("Граф должен быть 2-вершинно-связным");
        }

        private bool Is2VertexConnected(GraphInterface<TNode, TEdge> graph, List<TNode> nodes, List<TEdge> edges, bool isDirected)
        {
            if (nodes.Count < 3) return false;

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

            foreach (var excludeNode in nodes)
            {
                string excludeName = excludeNode.GetName();

                HashSet<string> visited = new();
                Queue<string> queue = new();

                string start = null;
                foreach (var node in nodes)
                {
                    if (node.GetName() != excludeName)
                    {
                        start = node.GetName();
                        break;
                    }
                }

                if (start == null) continue;

                queue.Enqueue(start);
                visited.Add(start);
                visited.Add(excludeName);

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

                if (visited.Count < nodes.Count)
                    return false;
            }

            return true;
        }
    }
}

