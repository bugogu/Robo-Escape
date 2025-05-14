using UnityEngine;

public class FirstLoad : MonoBehaviour
{
    [SerializeField] private float _loadDelay = 3f;
    void Awake()
    {
        MaskTransitions.TransitionManager.Instance.LoadLevel("Menu",_loadDelay);
    }
}