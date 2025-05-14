using Unity.VisualScripting;
using UnityEngine;

public class VisionCone : MonoBehaviour
{
    public enum VisionType {Turret, Drone}
    public float VisionRange;
    
    [HideInInspector] public bool PlayerSpotted;
    [HideInInspector] public float InitialRange;

    [SerializeField] private VisionType _visionType;
    [SerializeField] Material _visionConeMaterial;
    [SerializeField] float _visionAngle = 20f;
    [SerializeField] LayerMask _visionObstructingLayer;//layer with objects that obstruct the enemy view, like walls, for example
    [SerializeField] int _visionConeResolution = 120;//the vision cone will be made up of triangles, the higher this value is the pretier the vision cone will be
    [SerializeField] private Color _spottedColor;
    [SerializeField] private Color _normalColor;

    private Mesh _visionConeMesh;
    private MeshFilter _meshFilter;

    private Turret _turret;
    private Drone _drone;
    
    //Create all of these variables, most of them are self explanatory, but for the ones that aren't i've added a comment to clue you in on what they do
    //for the ones that you dont understand dont worry, just follow along
    void Start()
    {
        transform.AddComponent<MeshRenderer>().material = _visionConeMaterial;
        _meshFilter = transform.AddComponent<MeshFilter>();
        _visionConeMesh = new Mesh();
        _visionAngle *= Mathf.Deg2Rad;

        InitialRange = VisionRange;

        if (_visionType == VisionType.Turret)
            _turret = transform.parent.GetComponent<Turret>();
        else if (_visionType == VisionType.Drone)
            _drone = transform.parent.GetComponent<Drone>();
    }

    
    void Update()
    {
        PlayerSpotted = false;
        DrawVisionCone();
        ChangeVisionColor(PlayerSpotted);
    }

    void DrawVisionCone()
    {
        int[] triangles = new int[(_visionConeResolution - 1) * 3];
        Vector3[] vertices = new Vector3[_visionConeResolution + 1];
        
        vertices[0] = Vector3.zero;
        var currentAngle = -_visionAngle / 2;
        var angleIcrement = _visionAngle / (_visionConeResolution - 1);

        float sine;
        float cosine;
    
        for (int i = 0; i < _visionConeResolution; i++)
        {
            sine = Mathf.Sin(currentAngle);
            cosine = Mathf.Cos(currentAngle);

            var raycastDirection = (transform.forward * cosine) + (transform.right * sine);
            var vertForward = (Vector3.forward * cosine) + (Vector3.right * sine);
    
            // Önce duvar var mı diye kontrol et Önemli!: _visionObstructingLayer da player ve obstacle seçili hata varsa player ı çıkart.
            if (Physics.Raycast(transform.position, raycastDirection, out RaycastHit hit, VisionRange, _visionObstructingLayer))
            {
                // Duvar varsa, bu mesafede bir şey göremez
                vertices[i + 1] = vertForward * hit.distance;
    
                // Duvarın arkasında oyuncu var mı diye kontrol et
                if (Physics.Raycast(transform.position, raycastDirection, out RaycastHit playerHit, hit.distance, LayerMask.GetMask("Player")))
                {
                    PlayerSpotted = true;
    
                    DoSomething(playerHit.transform);
                }
            }
            else
            {
                // Duvar yoksa, direkt oyuncu var mı diye kontrol et
                if (Physics.Raycast(transform.position, raycastDirection, out RaycastHit playerHit, VisionRange, LayerMask.GetMask("Player")))
                {
                    PlayerSpotted = true;
                    vertices[i + 1] = vertForward * playerHit.distance;
    
                    DoSomething(playerHit.transform);
                }
                else
                {
                    vertices[i + 1] = vertForward * VisionRange;
                }
            }
    
            currentAngle += angleIcrement;
        }
    
        for (int i = 0, j = 0; i < triangles.Length; i += 3, j++)
        {
            triangles[i] = 0;
            triangles[i + 1] = j + 1;
            triangles[i + 2] = j + 2;
        }
    
        _visionConeMesh.Clear();
        _visionConeMesh.vertices = vertices;
        _visionConeMesh.triangles = triangles;
        _meshFilter.mesh = _visionConeMesh;
    }

    public void ChangeVisionColor(bool PlayerSpotted)
    {
        if (PlayerSpotted)
            _visionConeMaterial.color = _spottedColor;
        else
            _visionConeMaterial.color = _normalColor;
    }

    private void DoSomething(Transform info)
    {
        if(_visionType == VisionType.Turret)
            _turret.PlayerSpotted(info.transform);
        else if (_visionType == VisionType.Drone)
            _drone.PlayerSpotted();
    }
}
