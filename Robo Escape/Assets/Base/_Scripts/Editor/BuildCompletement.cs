using UnityEngine;

public class BuildCompletement : MonoBehaviour
{
    [UnityEditor.Callbacks.PostProcessBuild(1)]
    public static void OnPostProcessBuild(UnityEditor.BuildTarget target, string path)
    {
        if (target == UnityEditor.BuildTarget.Android)
        {
            var logEntries = System.Type.GetType("UnityEditor.LogEntries, UnityEditor.dll");
            logEntries.GetMethod("Clear").Invoke(null, null);
        }
    }
}