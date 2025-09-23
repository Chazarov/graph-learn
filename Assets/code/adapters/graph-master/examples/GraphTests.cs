using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace GraphMaster.Examples
{
    /// <summary>
    /// Простые тесты для создания графа и удаления ребер/вершин
    /// </summary>
    public class GraphTests
    {
        /// <summary>
        /// Тест 1: Создание простого графа с вершинами и ребрами
        /// </summary>
        public static void TestGraphCreation()
        {
            Debug.Log("=== Тест 1: Создание графа ===");
            
            try
            {
                // Создаем взвешенный ориентированный граф
                var graph = new Graph();
                graph.MakeOriented();
                graph.MakeWeighed();
                
                // Добавляем вершины
                var node1 = graph.AddNode(1, "Вершина A");
                var node2 = graph.AddNode(2, "Вершина B");
                var node3 = graph.AddNode(3, "Вершина C");
                
                // Добавляем ребра
                var edge1 = new GraphEdge(node1, node2, graph, 0);
                edge1.SetWeight(5.0f);
                
                var edge2 = new GraphEdge(node2, node3, graph, 1);
                edge2.SetWeight(3.0f);
                
                var edge3 = new GraphEdge(node1, node3, graph, 2);
                edge3.SetWeight(8.0f);
                
                Debug.Log($"✓ Граф создан успешно!");
                Debug.Log($"  - Количество вершин: {graph.GetNodeCount()}");
                Debug.Log($"  - Вершина 1 имеет {node1.GetEdges().Count} ребер");
                Debug.Log($"  - Вершина 2 имеет {node2.GetEdges().Count} ребер");
                Debug.Log($"  - Вес ребра 1→2: {edge1.GetWeight()}");
                Debug.Log("=== Тест 1 пройден ===\n");
            }
            catch (Exception ex)
            {
                Debug.LogError($"✗ Тест 1 провален: {ex.Message}");
                Debug.LogError($"✗ Тест 1 Stactrase: {ex.StackTrace}");
            }
        }

        /// <summary>
        /// Тест 2: Удаление ребер из графа
        /// </summary>
        public static void TestEdgeDeletion()
        {
            Debug.Log("=== Тест 2: Удаление ребер ===");
            
            try
            {
                // Создаем простой граф
                var graph = new Graph();
                var node1 = graph.AddNode(1, "Start");
                var node2 = graph.AddNode(2, "Middle");
                var node3 = graph.AddNode(3, "End");
                
                // Добавляем ребра
                var edge1 = new GraphEdge(node1, node2, graph);
                var edge2 = new GraphEdge(node2, node3, graph);
                var edge3 = new GraphEdge(node1, node3, graph);
                
                Debug.Log($"Начальное состояние:");
                Debug.Log($"  - Вершина 1 имеет {node1.GetEdges().Count} ребер");
                Debug.Log($"  - Вершина 2 имеет {node2.GetEdges().Count} ребер");
                
                // Удаляем ребро из node1
                node1.DisconnectEdge(edge1);
                
                Debug.Log($"После удаления ребра 1→2:");
                Debug.Log($"  - Вершина 1 имеет {node1.GetEdges().Count} ребер");
                Debug.Log($"  - Вершина 2 имеет {node2.GetEdges().Count} ребер");
                
                // Удаляем еще одно ребро
                node1.DisconnectEdge(edge3);
                
                Debug.Log($"После удаления ребра 1→3:");
                Debug.Log($"  - Вершина 1 имеет {node1.GetEdges().Count} ребер");
                
                Debug.Log("✓ Ребра удалены успешно!");
                Debug.Log("=== Тест 2 пройден ===\n");
            }
            catch (Exception ex)
            {
                Debug.LogError($"✗ Тест 2 провален: {ex.Message}");
            }
        }

        /// <summary>
        /// Тест 3: Удаление вершин из графа
        /// </summary>
        public static void TestNodeDeletion()
        {
            Debug.Log("=== Тест 3: Удаление вершин ===");
            
            try
            {
                // Создаем граф с несколькими вершинами
                var graph = new Graph();
                var node1 = graph.AddNode(1, "Node A");
                var node2 = graph.AddNode(2, "Node B");
                var node3 = graph.AddNode(3, "Node C");
                var node4 = graph.AddNode(4, "Node D");
                
                // Добавляем ребра
                var edge1 = new GraphEdge(node1, node2, graph);
                var edge2 = new GraphEdge(node2, node3, graph);
                var edge3 = new GraphEdge(node3, node4, graph);
                var edge4 = new GraphEdge(node1, node4, graph);
                
                Debug.Log($"Начальное состояние:");
                Debug.Log($"  - Количество вершин в графе: {graph.GetNodeCount()}");
                Debug.Log($"  - Вершина 2 имеет {node2.GetEdges().Count} ребер");
                
                // Удаляем вершину 2 (это должно удалить все связанные с ней ребра)
                node2.DeleteNode();
                
                Debug.Log($"После удаления вершины 2:");
                Debug.Log($"  - Количество вершин в графе: {graph.GetNodeCount()}");
                
                // Проверяем, что вершина действительно удалена
                try
                {
                    graph.GetNode(2); // Должно выбросить исключение
                    Debug.LogError("✗ Вершина 2 все еще существует!");
                }
                catch (NodeNotFoundException)
                {
                    Debug.Log("✓ Вершина 2 успешно удалена из графа");
                }
                
                // Удаляем еще одну вершину
                graph.DeleteNode(4);
                
                Debug.Log($"После удаления вершины 4:");
                Debug.Log($"  - Количество вершин в графе: {graph.GetNodeCount()}");
                
                Debug.Log("✓ Вершины удалены успешно!");
                Debug.Log("=== Тест 3 пройден ===\n");
            }
            catch (Exception ex)
            {
                Debug.LogError($"✗ Тест 3 провален: {ex.Message}");
                Debug.LogError("StackTrace: " + ex.StackTrace);
            }
        }

        /// <summary>
        /// Запуск всех тестов
        /// </summary>
        public static void RunAllTests()
        {
            Debug.Log("========== ЗАПУСК ТЕСТОВ ГРАФА ==========");
            
            TestGraphCreation();
            TestEdgeDeletion();
            TestNodeDeletion();
            
            Debug.Log("========== ВСЕ ТЕСТЫ ЗАВЕРШЕНЫ ==========");
        }
    }
}
