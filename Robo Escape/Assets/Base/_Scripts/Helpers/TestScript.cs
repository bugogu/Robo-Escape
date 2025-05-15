using UnityEngine;

public class TestScript : MonoBehaviour
{
    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab)) 
        {
            GameManager.Instance.ChangeGameState(GameState.Lose);
        }
    }
}