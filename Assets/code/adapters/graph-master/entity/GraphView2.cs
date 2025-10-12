using Domain;
using GraphMaster;
using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace GrapMaster {
    public class GraphView2: GraphInterface
    {
        private List<Positioned2Node> nodes = new List<Positioned2Node>();

        private GraphInterface graph;


        public GraphView2(Graph graph)
        {
            this.graph = graph;
        }

        public GraphView2(): this(new Graph())
        { }

        public List<Positioned2Node> GetNodes()
        {
            return this.nodes; 
        }




        //  ����������� ������� ������������

        public void DistributeByAPowerAlgoritm(double width, double height, float square)
        {
            Func<double, double, double> getRepulsionForce = (d, k) => Math.Pow(k, 2) / d;
            Func<double, double, double> getAttractionForce = (d, k) => Math.Pow(d, 2) / k;

            List<Vector2> displacements = new List<Vector2>(new Vector2[this.nodes.Count]);

            double area = width * height;
            double k = calculateOptimalEdgeLength(width, height);
            double temperature = width / 10; // ��������� "�����������" ��� ����������

            int iterations = 100;

            for (int iter = 0; iter < iterations; iter++)
            {
                // �������� ��������
                for (int i = 0; i < this.nodes.Count; i++)
                    displacements[i] = Vector2.Zero;

                // ��������� ������������� ���� ����� ����� ���������
                for (int v = 0; v < this.nodes.Count; v++)
                {
                    for (int u = v + 1; u < this.nodes.Count; u++)
                    {
                        Vector2 delta = this.nodes[v].GetPosition() - this.nodes[u].GetPosition();
                        double distance = Math.Max(delta.Length(), 0.01); // ����� �������� ������� �� 0
                        Vector2 direction = Vector2.Normalize(delta);

                        double force = getRepulsionForce(distance, k);
                        Vector2 displacementForce = direction * (float)force;

                        displacements[v] += displacementForce;
                        displacements[u] -= displacementForce;
                    }
                }

                // ��������� ������������� ���� ��� ������, ��������� �������
                for (int v = 0; v < this.nodes.Count; v++)
                {
                    var edges = this.nodes[v].GetNodeEdges();
                    foreach (var edge in edges)
                    {
                        var otherNode = edge.GetSourceNode() == this.nodes[v].GetGraphNode() ?
                            edge.GetTargetNode() : edge.GetSourceNode();

                        int u = this.nodes.FindIndex(n => n.GetGraphNode() == otherNode);
                        if (u < 0) continue;

                        Vector2 delta = this.nodes[v].GetPosition() - this.nodes[u].GetPosition();
                        double distance = Math.Max(delta.Length(), 0.01);
                        Vector2 direction = Vector2.Normalize(delta);

                        double force = getAttractionForce(distance, k);
                        Vector2 displacementForce = -direction * (float)force;

                        displacements[v] += displacementForce;
                        displacements[u] -= displacementForce;
                    }
                }

                // ��������� ������� � ������������ ���� � �������� �������
                for (int v = 0; v < this.nodes.Count; v++)
                {
                    Vector2 displacement = displacements[v];

                    // ������������ ����� �������� �� "�����������"
                    if (displacement.Length() > temperature)
                        displacement = Vector2.Normalize(displacement) * (float)temperature;

                    Vector2 newPos = this.nodes[v].GetPosition() + displacement;

                    // ������������ � ������ ��������� �������
                    newPos.X = Math.Clamp(newPos.X, 0, (float)width);
                    newPos.Y = Math.Clamp(newPos.Y, 0, (float)height);

                    this.nodes[v].SetPosition(newPos);
                }

                // ���������� ��������� "�����������"
                temperature *= 0.95;
            }
        }

        private double calculateOptimalEdgeLength(double width, double height, double scale = 1.0)
        {
            int nodesCount = this.nodes.Count;
            double area = width * height;
            if (nodesCount == 0)
                return 0;
            double k = scale * Math.Sqrt(area / nodesCount);
            return k;
        }

        public bool AllowedParrallelEdges()
        {
            return graph.AllowedParrallelEdges();
        }

        public bool AllowedNegativeEdges()
        {
            return graph.AllowedNegativeEdges();
        }

        public bool AllowedLoops()
        {
            return graph.AllowedLoops();
        }

        public bool IsOriented()
        {
            return graph.IsOriented();
        }

        public bool IsWeighed()
        {
            return graph.IsWeighed();
        }

        public bool HasNodes()
        {
            return graph.HasNodes();
        }

        public void MakeOriented()
        {
            graph.MakeOriented();
        }

        public void MakeParralel()
        {
            graph.MakeParralel();
        }

        public void MakeWeighed()
        {
            graph.MakeWeighed();
        }

        public void AllowNegative()
        {
            graph.AllowNegative();
        }

        public void AllowLoops()
        {
            graph.AllowLoops();
        }

        List<GraphNodeInterface> GraphInterface.GetNodes()
        {
            return graph.GetNodes();
        }

        public int GetNodeCount()
        {
            return graph.GetNodeCount();
        }


        public void DeleteNode(int nodeNumber)
        {
            graph.DeleteNode(nodeNumber);
        }

        GraphNodeInterface AddNode(GraphNodeInterface node)
        {
            GraphNodeInterface node1 =  graph.AddNode(node);
            return new Positioned2Node(node1);
        }

        public GraphNodeInterface AddNode(string name)
        {
            return this.AddNode(name, "");
        }

        public GraphNodeInterface AddNode(string name, string description)
        {
            GraphNodeInterface node = new GraphNode(this.graph);

            return this.AddNode(node);
        }
    } 


    
}



