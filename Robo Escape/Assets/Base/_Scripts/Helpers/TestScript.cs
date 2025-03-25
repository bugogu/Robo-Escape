using UnityEngine;
using UnityEditor;

public class TestScript : MonoBehaviour
{
    
#if UNITY_EDITOR
    
    [MenuItem("Test Methods/Test")]
    private static void Test()
    {
        Debug.Log("Test");
    }

#endif

}
