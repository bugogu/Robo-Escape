using UnityEngine;

public class TestScript : MonoBehaviour
{

#if UNITY_EDITOR

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.W)) 
        {
            GameManager.Instance.ChangeGameState(GameState.Lose);
        }
        if(Input.GetKeyDown(KeyCode.S)) 
        {
            GameManager.Instance.ChangeGameState(GameState.Win);
        }
    }

#endif

}
