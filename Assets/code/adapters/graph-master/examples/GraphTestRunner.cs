using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GraphMaster.Examples
{
    /// <summary>
    /// Компонент для запуска тестов графа в Unity
    /// </summary>
    public class GraphTestRunner : MonoBehaviour
    {
        [Header("Настройки тестов")]
        [SerializeField] private bool runOnStart = true;
        [SerializeField] private bool runTestCreation = true;
        [SerializeField] private bool runTestEdgeDeletion = true;
        [SerializeField] private bool runTestNodeDeletion = true;

        void Start()
        {
            if (runOnStart)
            {
                RunSelectedTests();
            }
        }

        /// <summary>
        /// Запуск выбранных тестов
        /// </summary>
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

            Debug.Log("=== ТЕСТЫ ЗАВЕРШЕНЫ ===");
        }

        /// <summary>
        /// Запуск всех тестов (можно вызвать из Inspector)
        /// </summary>
        [ContextMenu("Запустить все тесты")]
        public void RunAllTests()
        {
            GraphTests.RunAllTests();
        }

        /// <summary>
        /// Запуск только теста создания графа
        /// </summary>
        [ContextMenu("Тест: Создание графа")]
        public void RunCreationTest()
        {
            GraphTests.TestGraphCreation();
        }

        /// <summary>
        /// Запуск только теста удаления ребер
        /// </summary>
        [ContextMenu("Тест: Удаление ребер")]
        public void RunEdgeDeletionTest()
        {
            GraphTests.TestEdgeDeletion();
        }

        /// <summary>
        /// Запуск только теста удаления вершин
        /// </summary>
        [ContextMenu("Тест: Удаление вершин")]
        public void RunNodeDeletionTest()
        {
            GraphTests.TestNodeDeletion();
        }
    }
}
