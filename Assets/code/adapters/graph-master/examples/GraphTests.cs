using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Domain;

namespace GraphMaster.Examples
{
    public class GraphTests
    {
        public static void TestGraphCreation()
        {
            Debug.Log("=== Тест 1: Создание графа ===");
            
            try
            {
                var graph = new MyGraph<GraphNode, GraphEdge>();
                
                var node1 = new GraphNode("A");
                var node2 = new GraphNode("B");
                var node3 = new GraphNode("C");
                
                graph.AddNode(node1);
                graph.AddNode(node2);
                graph.AddNode(node3);
                
                var edge1 = new GraphEdge(node1, node2, 5.0f);
                var edge2 = new GraphEdge(node2, node3, 3.0f);
                var edge3 = new GraphEdge(node1, node3, 8.0f);
                
                graph.AddEdge(edge1);
                graph.AddEdge(edge2);
                graph.AddEdge(edge3);
                
                if (graph.GetNodeCount() != 3)
                {
                    throw new TestFailedException("Неверное количество вершин");
                }
                if (graph.GetEdges().Count != 3)
                {
                    throw new TestFailedException("Неверное количество ребер");
                }
                if (node1.GetEdges().Count != 2)
                {
                    throw new TestFailedException("Неверное количество ребер у вершины A");
                }
                if (node2.GetEdges().Count != 2)
                {
                    throw new TestFailedException("Неверное количество ребер у вершины B");
                }
                if (edge1.GetWeight() != 5.0f)
                {
                    throw new TestFailedException("Неверный вес ребра A→B");
                }
                if (edge2.GetWeight() != 3.0f)
                {
                    throw new TestFailedException("Неверный вес ребра B→C");
                }
                if (edge3.GetWeight() != 8.0f)
                {
                    throw new TestFailedException("Неверный вес ребра A→C");
                }
                
                Debug.Log("=== Тест 1 пройден ===\n");
            }
            catch (Exception ex)
            {
                Debug.LogError($"Тест 1 провален: {ex.Message}");
                Debug.LogError($"StackTrace: {ex.StackTrace}");
            }
        }

        public static void TestEdgeDeletion()
        {
            Debug.Log("=== Тест 2: Удаление ребер ===");
            
            try
            {
                var graph = new MyGraph<GraphNode, GraphEdge>();
                
                var node1 = new GraphNode("Start");
                var node2 = new GraphNode("Middle");
                var node3 = new GraphNode("End");
                
                graph.AddNode(node1);
                graph.AddNode(node2);
                graph.AddNode(node3);
                
                var edge1 = new GraphEdge(node1, node2);
                var edge2 = new GraphEdge(node2, node3);
                var edge3 = new GraphEdge(node1, node3);
                
                graph.AddEdge(edge1);
                graph.AddEdge(edge2);
                graph.AddEdge(edge3);
                
                if (node1.GetEdges().Count != 2)
                {
                    throw new TestFailedException("Неверное количество ребер у Start в начале");
                }
                if (node2.GetEdges().Count != 2)
                {
                    throw new TestFailedException("Неверное количество ребер у вершины 'Middle' в начале");
                }
                
                node1.DisconnectEdge(edge1);
                
                if (node1.GetEdges().Count != 1)
                {
                    throw new TestFailedException("Неверное количество ребер у вершины 'Start' после удаления edge1");
                }
                if (node2.GetEdges().Count != 2)
                {
                    throw new TestFailedException("Неверное количество ребер у вершины 'Middle' после удаления edge1");
                }
                
                node1.DisconnectEdge(edge3);
                
                if (node1.GetEdges().Count != 0)
                {
                    throw new TestFailedException("Неверное количество ребер у вершины 'Start' после удаления edge3");
                }
                
                Debug.Log("=== Тест 2 пройден ===\n");
            }
            catch (Exception ex)
            {
                Debug.LogError($"Тест 2 провален: {ex.Message}");
                Debug.LogError($"StackTrace: {ex.StackTrace}");
            }
        }

        public static void TestNodeDeletion()
        {
            Debug.Log("=== Тест 3: Удаление вершин ===");
            
            try
            {
                var graph = new MyGraph<GraphNode, GraphEdge>();
                
                var node1 = new GraphNode("A");
                var node2 = new GraphNode("B");
                var node3 = new GraphNode("C");
                var node4 = new GraphNode("D");
                
                graph.AddNode(node1);
                graph.AddNode(node2);
                graph.AddNode(node3);
                graph.AddNode(node4);
                
                var edge1 = new GraphEdge(node1, node2);
                var edge2 = new GraphEdge(node2, node3);
                var edge3 = new GraphEdge(node3, node4);
                var edge4 = new GraphEdge(node1, node4);
                
                graph.AddEdge(edge1);
                graph.AddEdge(edge2);
                graph.AddEdge(edge3);
                graph.AddEdge(edge4);
                
                if (graph.GetNodeCount() != 4)
                {
                    throw new TestFailedException("Неверное количество вершин в начале");
                }
                if (graph.GetEdges().Count != 4)
                {
                    throw new TestFailedException("Неверное количество ребер в начале");
                }
                if (node2.GetEdges().Count != 2)
                {
                    throw new TestFailedException("Неверное количество ребер у вершины B");
                }
                
                graph.DeleteNode("B");
                
                if (graph.GetNodeCount() != 3)
                {
                    throw new TestFailedException("Неверное количество вершин после удаления B");
                }
                if (graph.GetEdges().Count != 2)
                {
                    throw new TestFailedException("Неверное количество ребер после удаления B");
                }
                
                try
                {
                    graph.GetNode("B");
                    throw new TestFailedException("Неудалось удалить вершину. Вершина B все еще существует");
                }
                catch (NodeNotFoundException)
                {
                }
                
                graph.DeleteNode("D");
                
                if (graph.GetNodeCount() != 2)
                {
                    throw new TestFailedException("Неверное количество вершин после удаления D");
                }
                if (graph.GetEdges().Count != 0)
                {
                    throw new TestFailedException("Неверное количество ребер после удаления D");
                }
                
                Debug.Log("=== Тест 3 пройден ===\n");
            }
            catch (Exception ex)
            {
                Debug.LogError($"Тест 3 провален: {ex.Message}");
                Debug.LogError($"StackTrace: {ex.StackTrace}");
            }
        }


        public static void TestCompleteCleanup()
        {
            Debug.Log("=== Тест 4: Полная очистка графа ===");

            try
            {
                var graph = new MyGraph<GraphNode, GraphEdge>();
                
                var node1 = new GraphNode("N1");
                var node2 = new GraphNode("N2");
                var node3 = new GraphNode("N3");
                var node4 = new GraphNode("N4");
                var node5 = new GraphNode("N5");
                
                graph.AddNode(node1);
                graph.AddNode(node2);
                graph.AddNode(node3);
                graph.AddNode(node4);
                graph.AddNode(node5);
                
                var edge1 = new GraphEdge(node1, node2);
                var edge2 = new GraphEdge(node2, node3);
                var edge3 = new GraphEdge(node3, node4);
                var edge4 = new GraphEdge(node4, node5);
                var edge5 = new GraphEdge(node1, node5);
                
                graph.AddEdge(edge1);
                graph.AddEdge(edge2);
                graph.AddEdge(edge3);
                graph.AddEdge(edge4);
                graph.AddEdge(edge5);
                
                if (graph.GetNodeCount() != 5)
                {
                    throw new TestFailedException("Неверное количество вершин в начале");
                }
                if (graph.GetEdges().Count != 5)
                {
                    throw new TestFailedException("Неверное количество ребер в начале");
                }
                
                node1.DisconnectEdge(edge1);
                node2.DisconnectEdge(edge2);
                
                if (graph.GetEdges().Count != 5)
                {
                    throw new TestFailedException("Неверное количество ребер после отсоединения");
                }
                if (node1.GetEdges().Count != 1)
                {
                    throw new TestFailedException("Неверное количество ребер у N1");
                }
                
                graph.DeleteNode("N3");
                
                if (graph.GetNodeCount() != 4)
                {
                    throw new TestFailedException("Неверное количество вершин после удаления N3");
                }
                if (graph.GetEdges().Count != 3)
                {
                    throw new TestFailedException("Неверное количество ребер после удаления N3");
                }
                
                graph.DeleteNode("N1");
                graph.DeleteNode("N2");
                graph.DeleteNode("N4");
                graph.DeleteNode("N5");
                
                if (graph.GetNodeCount() != 0)
                {
                    throw new TestFailedException("Неверное количество вершин после полной очистки");
                }
                if (graph.GetEdges().Count != 0)
                {
                    throw new TestFailedException("Неверное количество ребер после полной очистки");
                }
                if (graph.HasNodes())
                {
                    throw new TestFailedException("Граф не пуст после удаления всех вершин");
                }
                
                Debug.Log("=== Тест 4 пройден ===\n");
            }
            catch (Exception ex)
            {
                Debug.LogError($"Тест 4 провален: {ex.Message}");
                Debug.LogError($"StackTrace: {ex.StackTrace}");
            }
        }

        public static void TestForceDirectedLine()
        {
            Debug.Log("=== Тест 5: Силовое распределение линии ===");

            try
            {
                var graph = new MyGraph<Positioned2Node, GraphEdge>();
                
                var node1 = new Positioned2Node(System.Numerics.Vector2.Zero, "A");
                var node2 = new Positioned2Node(System.Numerics.Vector2.Zero, "B");
                var node3 = new Positioned2Node(System.Numerics.Vector2.Zero, "C");
                var node4 = new Positioned2Node(System.Numerics.Vector2.Zero, "D");
                var node5 = new Positioned2Node(System.Numerics.Vector2.Zero, "E");
                
                graph.AddNode(node1);
                graph.AddNode(node2);
                graph.AddNode(node3);
                graph.AddNode(node4);
                graph.AddNode(node5);
                
                var edge1 = new GraphEdge(node1, node2);
                var edge2 = new GraphEdge(node2, node3);
                var edge3 = new GraphEdge(node3, node4);
                var edge4 = new GraphEdge(node4, node5);
                
                graph.AddEdge(edge1);
                graph.AddEdge(edge2);
                graph.AddEdge(edge3);
                graph.AddEdge(edge4);
                
                var algorithm = new AlghoritmMaster();
                algorithm.MakeForceDirectedDistribution(graph);
                
                var pos1 = node1.GetPosition();
                var pos5 = node5.GetPosition();
                var pos2 = node2.GetPosition();
                var pos3 = node3.GetPosition();
                
                float distanceEnds = System.Numerics.Vector2.Distance(pos1, pos5);
                float distanceCenter = System.Numerics.Vector2.Distance(pos2, pos3);
                
                if (distanceEnds <= distanceCenter)
                {
                    throw new TestFailedException($"Расстояние между крайними узлами ({distanceEnds}) должно быть больше расстояния между центральными ({distanceCenter})");
                }
                
                Debug.Log("=== Тест 5 пройден ===\n");
            }
            catch (Exception ex)
            {
                Debug.LogError($"Тест 5 провален: {ex.Message}");
                Debug.LogError($"StackTrace: {ex.StackTrace}");
            }
        }

        public static void TestForceDirectedRingWithDiagonal()
        {
            Debug.Log("=== Тест 6: Силовое распределение кольца с диагональю ===");

            try
            {
                var graph = new MyGraph<Positioned2Node, GraphEdge>();
                
                var nodeA = new Positioned2Node(System.Numerics.Vector2.Zero, "A");
                var nodeB = new Positioned2Node(System.Numerics.Vector2.Zero, "B");
                var nodeC = new Positioned2Node(System.Numerics.Vector2.Zero, "C");
                var nodeD = new Positioned2Node(System.Numerics.Vector2.Zero, "D");
                
                graph.AddNode(nodeA);
                graph.AddNode(nodeB);
                graph.AddNode(nodeC);
                graph.AddNode(nodeD);
                
                var edge1 = new GraphEdge(nodeA, nodeB);
                var edge2 = new GraphEdge(nodeB, nodeC);
                var edge3 = new GraphEdge(nodeC, nodeD);
                var edge4 = new GraphEdge(nodeD, nodeA);
                var edgeDiagonal = new GraphEdge(nodeA, nodeC);
                
                graph.AddEdge(edge1);
                graph.AddEdge(edge2);
                graph.AddEdge(edge3);
                graph.AddEdge(edge4);
                graph.AddEdge(edgeDiagonal);
                
                var algorithm = new AlghoritmMaster();
                algorithm.MakeForceDirectedDistribution(graph);
                
                var posA = nodeA.GetPosition();
                var posB = nodeB.GetPosition();
                var posC = nodeC.GetPosition();
                var posD = nodeD.GetPosition();
                
                float distanceBD = System.Numerics.Vector2.Distance(posB, posD);
                float distanceAB = System.Numerics.Vector2.Distance(posA, posB);
                float distanceBC = System.Numerics.Vector2.Distance(posB, posC);
                float distanceCD = System.Numerics.Vector2.Distance(posC, posD);
                float distanceDA = System.Numerics.Vector2.Distance(posD, posA);
                float distanceAC = System.Numerics.Vector2.Distance(posA, posC);
                
                if (distanceBD <= distanceAB || distanceBD <= distanceBC || 
                    distanceBD <= distanceCD || distanceBD <= distanceDA || distanceBD <= distanceAC)
                {
                    throw new TestFailedException($"Расстояние B-D ({distanceBD}) должно быть максимальным");
                }
                
                Debug.Log("=== Тест 6 пройден ===\n");
            }
            catch (Exception ex)
            {
                Debug.LogError($"Тест 6 провален: {ex.Message}");
                Debug.LogError($"StackTrace: {ex.StackTrace}");
            }
        }

        public static void TestForceDirectedUniquePositions()
        {
            Debug.Log("=== Тест 7: Все узлы имеют уникальные позиции ===");

            try
            {
                var graph = new MyGraph<Positioned2Node, GraphEdge>();
                
                var node1 = new Positioned2Node(System.Numerics.Vector2.Zero, "N1");
                var node2 = new Positioned2Node(System.Numerics.Vector2.Zero, "N2");
                var node3 = new Positioned2Node(System.Numerics.Vector2.Zero, "N3");
                var node4 = new Positioned2Node(System.Numerics.Vector2.Zero, "N4");
                
                graph.AddNode(node1);
                graph.AddNode(node2);
                graph.AddNode(node3);
                graph.AddNode(node4);
                
                var edge1 = new GraphEdge(node1, node2);
                var edge2 = new GraphEdge(node2, node3);
                var edge3 = new GraphEdge(node3, node4);
                
                graph.AddEdge(edge1);
                graph.AddEdge(edge2);
                graph.AddEdge(edge3);
                
                var algorithm = new AlghoritmMaster();
                algorithm.MakeForceDirectedDistribution(graph);
                
                var positions = new List<System.Numerics.Vector2>
                {
                    node1.GetPosition(),
                    node2.GetPosition(),
                    node3.GetPosition(),
                    node4.GetPosition()
                };
                
                for (int i = 0; i < positions.Count; i++)
                {
                    for (int j = i + 1; j < positions.Count; j++)
                    {
                        float distance = System.Numerics.Vector2.Distance(positions[i], positions[j]);
                        if (distance < 0.1f)
                        {
                            throw new TestFailedException($"Узлы {i} и {j} имеют слишком близкие позиции (расстояние {distance})");
                        }
                    }
                }
                
                Debug.Log("=== Тест 7 пройден ===\n");
            }
            catch (Exception ex)
            {
                Debug.LogError($"Тест 7 провален: {ex.Message}");
                Debug.LogError($"StackTrace: {ex.StackTrace}");
            }
        }

        public static void RunAllTests()
        {
            Debug.Log("========== ЗАПУСК ТЕСТОВ ГРАФА ==========");
            
            TestGraphCreation();
            TestEdgeDeletion();
            TestNodeDeletion();
            TestCompleteCleanup();
            TestForceDirectedLine();
            TestForceDirectedRingWithDiagonal();
            TestForceDirectedUniquePositions();
            
            Debug.Log("========== ВСЕ ТЕСТЫ ЗАВЕРШЕНЫ ==========");
        }
    }
}
