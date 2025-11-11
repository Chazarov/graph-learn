using Domain;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Numerics;

namespace GraphMaster
{
    public class AlghoritmMaster
    {
        private float area = 1000f;
        private float C = 1f;
        private int maxIterations = 100;
        private float initialTemperature = 10f;
        private float coolingRate = 0.95f;

        public void MakeForceDirectedDistribution(GraphInterface<Positioned2Node, GraphEdge> graph)
        {
            if (!graph.HasNodes())
            {
                return;
            }

            List<Positioned2Node> nodes = graph.GetNodes();
            int nodeCount = nodes.Count;

            InitializeRandomPositions(nodes);

            float k = C * (float)Math.Sqrt(area / nodeCount);
            float temperature = initialTemperature;

            for (int iteration = 0; iteration < maxIterations; iteration++)
            {
                Dictionary<Positioned2Node, Vector2> forces = new Dictionary<Positioned2Node, Vector2>();

                foreach (var node in nodes)
                {
                    forces[node] = Vector2.Zero;
                }

                CalculateRepulsiveForces(nodes, forces, k);
                CalculateAttractiveForces(nodes, forces, k);

                UpdatePositions(nodes, forces, temperature);

                temperature *= coolingRate;

                if (temperature < 0.1f)
                {
                    break;
                }
            }
        }

        private void InitializeRandomPositions(List<Positioned2Node> nodes)
        {
            System.Random random = new System.Random();
            float range = (float)Math.Sqrt(area);

            foreach (var node in nodes)
            {
                float x = (float)(random.NextDouble() * range - range / 2);
                float y = (float)(random.NextDouble() * range - range / 2);
                node.SetPosition(new Vector2(x, y));
            }
        }

        private void CalculateRepulsiveForces(List<Positioned2Node> nodes, Dictionary<Positioned2Node, Vector2> forces, float k)
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                for (int j = i + 1; j < nodes.Count; j++)
                {
                    Positioned2Node v1 = nodes[i];
                    Positioned2Node v2 = nodes[j];

                    Vector2 pos1 = v1.GetPosition();
                    Vector2 pos2 = v2.GetPosition();

                    Vector2 delta = pos1 - pos2;
                    float distance = delta.Length();

                    if (distance < 0.01f)
                    {
                        distance = 0.01f;
                    }

                    float repulsiveForce = (k * k) / distance;
                    Vector2 force = Vector2.Normalize(delta) * repulsiveForce;

                    forces[v1] += force;
                    forces[v2] -= force;
                }
            }
        }

        private void CalculateAttractiveForces(List<Positioned2Node> nodes, Dictionary<Positioned2Node, Vector2> forces, float k)
        {
            foreach (var node in nodes)
            {
                List<GraphEdgeInterface> edges = node.GetEdges();

                foreach (var edge in edges)
                {
                    GraphNodeInterface otherNodeInterface = null;

                    if (edge.GetSourceNode() == node)
                    {
                        otherNodeInterface = edge.GetTargetNode();
                    }
                    else if (edge.GetTargetNode() == node)
                    {
                        otherNodeInterface = edge.GetSourceNode();
                    }

                    if (otherNodeInterface is Positioned2Node otherNode)
                    {
                        Vector2 pos1 = node.GetPosition();
                        Vector2 pos2 = otherNode.GetPosition();

                        Vector2 delta = pos2 - pos1;
                        float distance = delta.Length();

                        if (distance < 0.01f)
                        {
                            distance = 0.01f;
                        }

                        float attractiveForce = (distance * distance) / k;
                        Vector2 force = Vector2.Normalize(delta) * attractiveForce;

                        forces[node] += force;
                    }
                }
            }
        }

        private void UpdatePositions(List<Positioned2Node> nodes, Dictionary<Positioned2Node, Vector2> forces, float temperature)
        {
            foreach (var node in nodes)
            {
                Vector2 force = forces[node];
                float forceLength = force.Length();

                if (forceLength > 0)
                {
                    float displacement = Math.Min(forceLength, temperature);
                    Vector2 newPosition = node.GetPosition() + Vector2.Normalize(force) * displacement;
                    node.SetPosition(newPosition);
                }
            }
        }
    }

}

