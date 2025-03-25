using MaskTransitions;
using UnityEngine;

public class FirstLoad : MonoBehaviour
{
    [SerializeField] private float _loadDelay = 3f;
    void Awake()
    {
        TransitionManager.Instance.LoadLevel("Menu",_loadDelay);
    }
}
