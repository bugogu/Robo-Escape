using UnityEngine;

public class RotateY : MonoBehaviour
{
    [SerializeField] float rotationSpeed = 300f; 
    [SerializeField] bool x = false;
    [SerializeField] bool y = true;
    [SerializeField] bool z = false;

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
            if (x)
                transform.Rotate(rotationSpeed * Time.deltaTime, 0, 0);
            if (y)
                transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
            if (z)
                transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
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
