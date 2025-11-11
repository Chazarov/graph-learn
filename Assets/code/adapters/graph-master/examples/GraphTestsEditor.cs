#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace GraphMaster.Examples
{
    public static class GraphTestsEditor
    {
        [MenuItem("Graph Tests/Запустить все тесты")]
        public static void RunAllTests()
        {
            Debug.Log("Запуск всех тестов через меню...");
            GraphTests.RunAllTests();
        }

        [MenuItem("Graph Tests/Тест создания графа")]
        public static void RunCreationTest()
        {
            Debug.Log("Запуск теста создания графа...");
            GraphTests.TestGraphCreation();
        }

        [MenuItem("Graph Tests/Тест удаления ребер")]
        public static void RunEdgeDeletionTest()
        {
            Debug.Log("Запуск теста удаления ребер...");
            GraphTests.TestEdgeDeletion();
        }

        [MenuItem("Graph Tests/Тест удаления вершин")]
        public static void RunNodeDeletionTest()
        {
            Debug.Log("Запуск теста удаления вершин...");
            GraphTests.TestNodeDeletion();
        }

        [MenuItem("Graph Tests/Тест полной очистки")]
        public static void RunCompleteCleanupTest()
        {
            Debug.Log("Запуск теста полной очистки...");
            GraphTests.TestCompleteCleanup();
        }

        [MenuItem("Graph Tests/Тест силового распределения линии")]
        public static void RunForceDirectedLineTest()
        {
            Debug.Log("Запуск теста силового распределения линии...");
            GraphTests.TestForceDirectedLine();
        }

        [MenuItem("Graph Tests/Тест силового распределения кольца")]
        public static void RunForceDirectedRingTest()
        {
            Debug.Log("Запуск теста силового распределения кольца...");
            GraphTests.TestForceDirectedRingWithDiagonal();
        }

        [MenuItem("Graph Tests/Тест уникальных позиций")]
        public static void RunForceDirectedUniqueTest()
        {
            Debug.Log("Запуск теста уникальных позиций...");
            GraphTests.TestForceDirectedUniquePositions();
        }

        [MenuItem("Graph Tests/Очистить консоль")]
        public static void ClearConsole()
        {
            var assembly = System.Reflection.Assembly.GetAssembly(typeof(UnityEditor.Editor));
            var type = assembly.GetType("UnityEditor.LogEntries");
            var method = type.GetMethod("Clear");
            method.Invoke(new object(), null);
        }
    }
}
#endif
