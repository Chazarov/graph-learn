using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GraphMaster.Examples
{
    public class GraphTestRunner : MonoBehaviour
    {
        [Header("Настройки тестов")]
        [SerializeField] private bool runOnStart = true;
        [SerializeField] private bool runTestCreation = true;
        [SerializeField] private bool runTestEdgeDeletion = true;
        [SerializeField] private bool runTestNodeDeletion = true;
        [SerializeField] private bool runTestCompleteCleanup = true;
        [SerializeField] private bool runTestForceDirectedLine = true;
        [SerializeField] private bool runTestForceDirectedRing = true;
        [SerializeField] private bool runTestForceDirectedUnique = true;

        void Start()
        {
            if (runOnStart)
            {
                RunSelectedTests();
            }
        }

        public void RunSelectedTests()
        {
            Debug.Log("=== ЗАПУСК ТЕСТОВ ГРАФА ===");

            if (runTestCreation)
            {
                GraphTests.TestGraphCreation();
            }

            if (runTestEdgeDeletion)
            {
                GraphTests.TestEdgeDeletion();
            }

            if (runTestNodeDeletion)
            {
                GraphTests.TestNodeDeletion();
            }

            if (runTestCompleteCleanup)
            {
                GraphTests.TestCompleteCleanup();
            }

            if (runTestForceDirectedLine)
            {
                GraphTests.TestForceDirectedLine();
            }

            if (runTestForceDirectedRing)
            {
                GraphTests.TestForceDirectedRingWithDiagonal();
            }

            if (runTestForceDirectedUnique)
            {
                GraphTests.TestForceDirectedUniquePositions();
            }

            Debug.Log("=== ТЕСТЫ ЗАВЕРШЕНЫ ===");
        }

        [ContextMenu("Запустить все тесты")]
        public void RunAllTests()
        {
            GraphTests.RunAllTests();
        }

        [ContextMenu("Тест: Создание графа")]
        public void RunCreationTest()
        {
            GraphTests.TestGraphCreation();
        }

        [ContextMenu("Тест: Удаление ребер")]
        public void RunEdgeDeletionTest()
        {
            GraphTests.TestEdgeDeletion();
        }

        [ContextMenu("Тест: Удаление вершин")]
        public void RunNodeDeletionTest()
        {
            GraphTests.TestNodeDeletion();
        }

        [ContextMenu("Тест: Полная очистка")]
        public void RunCompleteCleanupTest()
        {
            GraphTests.TestCompleteCleanup();
        }

        [ContextMenu("Тест: Силовое распределение линии")]
        public void RunForceDirectedLineTest()
        {
            GraphTests.TestForceDirectedLine();
        }

        [ContextMenu("Тест: Силовое распределение кольца")]
        public void RunForceDirectedRingTest()
        {
            GraphTests.TestForceDirectedRingWithDiagonal();
        }

        [ContextMenu("Тест: Уникальные позиции")]
        public void RunForceDirectedUniqueTest()
        {
            GraphTests.TestForceDirectedUniquePositions();
        }
    }
}
