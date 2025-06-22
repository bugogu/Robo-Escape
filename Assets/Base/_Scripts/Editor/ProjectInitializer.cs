[UnityEditor.InitializeOnLoad]
public static class ProjectInitializer
{
    static ProjectInitializer() =>
           HandleConsole();

    public static void HandleConsole()
    {
        var logEntries = System.Type.GetType("UnityEditor.LogEntries, UnityEditor.dll");
        var clearMethod = logEntries.GetMethod("Clear");
        clearMethod.Invoke(null, null);  

        UnityEngine.Debug.Log("Babacik geldi.");  
    }
}