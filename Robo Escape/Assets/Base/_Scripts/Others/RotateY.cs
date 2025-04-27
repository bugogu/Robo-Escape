using UnityEngine;

public class RotateY : MonoBehaviour
{
    [SerializeField] float rotationSpeed = 300f; 

    private Camera _mainCamera;
    private Renderer _renderer;

    void Start()
    {
        _mainCamera = Camera.main;
        _renderer = GetComponent<Renderer>();
    }

    void Update()
    {
        if (IsVisibleToCamera())
        {
            transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
        }
    }

    bool IsVisibleToCamera()
    {
        if (_renderer == null || _mainCamera == null)
            return false;

        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(_mainCamera);
        return GeometryUtility.TestPlanesAABB(planes, _renderer.bounds);
    }
}
