using UnityEditor;
using UnityEngine.SceneManagement;

[InitializeOnLoad]
public static class SceneChangeHandler
{
    private static string lastSceneName;

    static SceneChangeHandler() =>
        EditorApplication.hierarchyChanged += OnSceneChanged;

    private static void OnSceneChanged()
    {
        if (SceneManager.GetActiveScene().name != lastSceneName)
        {
            lastSceneName = SceneManager.GetActiveScene().name;
            ClearConsole();
        }
    }
    
    private static void ClearConsole()
    {
        var logEntries = System.Type.GetType("UnityEditor.LogEntries, UnityEditor.dll");
        logEntries.GetMethod("Clear").Invoke(null, null);
    }
}
