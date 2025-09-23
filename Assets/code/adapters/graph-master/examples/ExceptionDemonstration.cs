using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace GraphMaster.Examples
{
    /// <summary>
    /// Класс для демонстрации работы системы исключений в графах
    /// </summary>
    public class ExceptionDemonstration
    {
        public static void DemonstrateExceptions()
        {
            Debug.Log("=== Демонстрация системы исключений GraphMaster ===");

            // 1. Демонстрация DuplicateNodeException
            try
            {
                var graph = new Graph();
                graph.AddNode(1, "Node1");
                graph.AddNode(1, "DuplicateNode"); // Должно выбросить исключение
            }
            catch (DuplicateNodeException ex)
            {
                Debug.Log($"✓ DuplicateNodeException: {ex.Message}");
            }

            // 2. Демонстрация NodeNotFoundException
            try
            {
                var graph = new Graph();
                graph.GetNode(999); // Должно выбросить исключение
            }
            catch (NodeNotFoundException ex)
            {
                Debug.Log($"✓ NodeNotFoundException: {ex.Message}");
            }

            // 3. Демонстрация NegativeEdgeNotAllowed
            try
            {
                var graph = new Graph();
                graph.MakeWeighed();
                // НЕ разрешаем отрицательные ребра
                var node1 = graph.AddNode(1);
                var node2 = graph.AddNode(2);
                var edge = new GraphEdge(node1, node2, graph, -5.0f); // Должно выбросить исключение
            }
            catch (NegativeEdgeNotAllowed ex)
            {
                Debug.Log($"✓ NegativeEdgeNotAllowed: {ex.Message}");
            }

            // 4. Демонстрация LoopNotAllowed
            try
            {
                var graph = new Graph();
                // НЕ разрешаем петли
                var node1 = graph.AddNode(1);
                var edge = new GraphEdge(node1, node1, graph); // Должно выбросить исключение
            }
            catch (LoopNotAllowed ex)
            {
                Debug.Log($"✓ LoopNotAllowed: {ex.Message}");
            }

            // 5. Демонстрация WeightRequiredException
            try
            {
                var graph = new Graph();
                graph.MakeWeighed();
                var node1 = graph.AddNode(1);
                var node2 = graph.AddNode(2);
                var edge = new GraphEdge(node1, node2, graph); // Ребро без веса во взвешенном графе
                edge.GetWeight(); // Должно выбросить исключение
            }
            catch (WeightRequiredException ex)
            {
                Debug.Log($"✓ WeightRequiredException: {ex.Message}");
            }

            // 6. Демонстрация WeightNotAllowedException
            try
            {
                var graph = new Graph();
                // Граф НЕ взвешенный
                var node1 = graph.AddNode(1);
                var node2 = graph.AddNode(2);
                var edge = new GraphEdge(node1, node2, graph, 10.0f); // Должно выбросить исключение
            }
            catch (WeightNotAllowedException ex)
            {
                Debug.Log($"✓ WeightNotAllowedException: {ex.Message}");
            }

            // 7. Демонстрация ParralelEdgesNotAllowed
            try
            {
                var graph = new Graph();
                graph.MakeOriented();
                // НЕ разрешаем параллельные ребра
                var node1 = graph.AddNode(1);
                var node2 = graph.AddNode(2);
                var edge1 = new GraphEdge(node1, node2, graph);
                var edge2 = new GraphEdge(node1, node2, graph); // Должно выбросить исключение
            }
            catch (ParralelEdgesNotAllowed ex)
            {
                Debug.Log($"✓ ParralelEdgesNotAllowed: {ex.Message}");
            }

            // 8. Демонстрация EmptyGraphException
            try
            {
                var graph = new Graph();
                graph.MakeWeighed();
                graph.MakeDijkstra(); // Должно выбросить исключение
            }
            catch (EmptyGraphException ex)
            {
                Debug.Log($"✓ EmptyGraphException: {ex.Message}");
            }

            // 9. Демонстрация InvalidGraphOperationException
            try
            {
                var graph = new Graph();
                // Граф НЕ взвешенный
                var node1 = graph.AddNode(1);
                graph.MakeDijkstra(); // Должно выбросить исключение
            }
            catch (InvalidGraphOperationException ex)
            {
                Debug.Log($"✓ InvalidGraphOperationException: {ex.Message}");
            }

            Debug.Log("=== Демонстрация завершена ===");
        }

        public static void DemonstrateCorrectUsage()
        {
            Debug.Log("=== Демонстрация корректного использования ===");

            // Создание взвешенного ориентированного графа с разрешенными петлями и отрицательными ребрами
            var graph = new Graph();
            graph.MakeOriented();
            graph.MakeWeighed();
            graph.AllowLoops();
            graph.AllowNegative();

            var node1 = graph.AddNode(1, "Start");
            var node2 = graph.AddNode(2, "Middle");
            var node3 = graph.AddNode(3, "End");

            var edge1 = new GraphEdge(node1, node2, graph, 5.0f);
            var edge2 = new GraphEdge(node2, node3, graph, -2.0f); // Отрицательное ребро разрешено
            var edge3 = new GraphEdge(node1, node1, graph, 1.0f);  // Петля разрешена

            Debug.Log($"✓ Граф создан успешно с {graph.GetNodeCount()} вершинами");
            Debug.Log("=== Корректное использование завершено ===");
        }
    }
}
