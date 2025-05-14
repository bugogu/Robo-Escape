using UnityEngine;

public class TestScript : MonoBehaviour
{

#if UNITY_EDITOR

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab)) 
        {
            GameManager.Instance.ChangeGameState(GameState.Lose);
        }
    }
#endif

}