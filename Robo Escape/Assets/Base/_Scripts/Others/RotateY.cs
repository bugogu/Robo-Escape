using UnityEngine;

public class RotateY : MonoBehaviour
{
    [SerializeField] float rotationSpeed = 300f; 

    void Update()
    {
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
    }
}
