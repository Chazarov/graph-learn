#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace GraphMaster.Examples
{
    /// <summary>
    /// Editor-скрипт для запуска тестов через меню Unity
    /// </summary>
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
